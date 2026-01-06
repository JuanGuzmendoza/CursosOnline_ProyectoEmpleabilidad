import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { createCourse, getCourseSummary, updateCourse } from '../api/courses';
import { Button } from '../components/Button';
import { Input } from '../components/Input';
import { Layout } from '../components/Layout';
import { ArrowLeft } from 'lucide-react';

export const CourseEditor: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const isEditing = !!id;
    const navigate = useNavigate();

    const [title, setTitle] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState('');

    useEffect(() => {
        if (isEditing) {
            const fetchCourse = async () => {
                try {
                    const data = await getCourseSummary(id);
                    setTitle(data.title);
                } catch (err) {
                    console.error('Failed to fetch course', err);
                    setError('Failed to load course details');
                }
            };
            fetchCourse();
        }
    }, [id, isEditing]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsLoading(true);
        setError('');

        try {
            if (isEditing) {
                await updateCourse(id, { title });
            } else {
                await createCourse({ title });
            }
            navigate('/');
        } catch (err) {
            console.error('Failed to save course', err);
            setError('Failed to save course');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Layout>
            <div className="max-w-2xl mx-auto">
                <Button variant="outline" onClick={() => navigate(-1)} className="mb-6 flex items-center gap-2">
                    <ArrowLeft className="w-4 h-4" />
                    Back
                </Button>

                <div className="card p-6">
                    <h1 className="text-2xl font-bold mb-6">
                        {isEditing ? 'Edit Course' : 'Create New Course'}
                    </h1>

                    {error && (
                        <div className="bg-red-50 text-red-500 p-3 rounded mb-4 text-sm">
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit}>
                        <Input
                            label="Course Title"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            required
                            placeholder="e.g., Advanced React Patterns"
                        />

                        <div className="flex justify-end gap-4 mt-6">
                            <Button type="button" variant="outline" onClick={() => navigate(-1)}>
                                Cancel
                            </Button>
                            <Button type="submit" isLoading={isLoading}>
                                {isEditing ? 'Update Course' : 'Create Course'}
                            </Button>
                        </div>
                    </form>
                </div>
            </div>
        </Layout>
    );
};
