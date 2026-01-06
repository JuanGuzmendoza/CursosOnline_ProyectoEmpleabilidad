import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { searchCourses, deleteCourse } from '../api/courses';
import { CourseStatus } from '../types';
import type { Course } from '../types';
import { Button } from '../components/Button';
import { Layout } from '../components/Layout';
import { Plus, Search, Edit, Trash2, Eye } from 'lucide-react';

export const Dashboard: React.FC = () => {
    const [courses, setCourses] = useState<Course[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [query, setQuery] = useState('');
    const [status, setStatus] = useState<CourseStatus | undefined>(undefined);

    const fetchCourses = async () => {
        setIsLoading(true);
        try {
            // Assuming the API returns an array of courses or a paginated result
            // Swagger says "OK" but no schema. Let's assume it returns { items: [], ... } or just []
            const data = await searchCourses(query, status);
            // Adjust based on actual API response
            setCourses(Array.isArray(data) ? data : data.items || []);
        } catch (error) {
            console.error('Failed to fetch courses', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchCourses();
    }, [query, status]);

    const handleDelete = async (id: string) => {
        if (window.confirm('Are you sure you want to delete this course?')) {
            try {
                await deleteCourse(id);
                fetchCourses();
            } catch (error) {
                console.error('Failed to delete course', error);
            }
        }
    };

    return (
        <Layout>
            <div className="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
                <h1 className="text-2xl font-bold">My Courses</h1>
                <Link to="/courses/new">
                    <Button className="flex items-center gap-2">
                        <Plus className="w-4 h-4" />
                        Create Course
                    </Button>
                </Link>
            </div>

            <div className="flex flex-col sm:flex-row gap-4 mb-6">
                <div className="relative flex-1">
                    <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-4 h-4" />
                    <input
                        type="text"
                        placeholder="Search courses..."
                        className="input pl-10"
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                    />
                </div>
                <select
                    className="input w-full sm:w-48"
                    value={status === undefined ? '' : status}
                    onChange={(e) => setStatus(e.target.value ? Number(e.target.value) as CourseStatus : undefined)}
                >
                    <option value="">All Status</option>
                    <option value={CourseStatus.Draft}>Draft</option>
                    <option value={CourseStatus.Published}>Published</option>
                </select>
            </div>

            {isLoading ? (
                <div className="text-center py-10">Loading...</div>
            ) : courses.length === 0 ? (
                <div className="text-center py-10 text-muted">
                    No courses found. Create one to get started!
                </div>
            ) : (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {courses.map((course) => (
                        <div key={course.id} className="card p-4 flex flex-col">
                            <div className="flex justify-between items-start mb-2">
                                <h3 className="font-bold text-lg truncate" title={course.title}>
                                    {course.title}
                                </h3>
                                <span className={`text-xs px-2 py-1 rounded-full ${course.status === CourseStatus.Published
                                    ? 'bg-green-100 text-green-800'
                                    : 'bg-gray-100 text-gray-800'
                                    }`}>
                                    {course.status === CourseStatus.Published ? 'Published' : 'Draft'}
                                </span>
                            </div>

                            <div className="mt-auto pt-4 flex justify-end gap-2">
                                <Link to={`/courses/${course.id}`}>
                                    <Button variant="outline" className="p-2" title="View Details">
                                        <Eye className="w-4 h-4" />
                                    </Button>
                                </Link>
                                <Link to={`/courses/${course.id}/edit`}>
                                    <Button variant="outline" className="p-2" title="Edit">
                                        <Edit className="w-4 h-4" />
                                    </Button>
                                </Link>
                                <Button
                                    variant="outline"
                                    className="p-2 text-red-500 hover:bg-red-50 border-red-200"
                                    onClick={() => handleDelete(course.id)}
                                    title="Delete"
                                >
                                    <Trash2 className="w-4 h-4" />
                                </Button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </Layout>
    );
};
