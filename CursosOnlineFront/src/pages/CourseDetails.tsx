import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getCourseSummary, publishCourse, unpublishCourse } from '../api/courses';
import { getLessonsByCourse, createLesson, updateLesson, deleteLesson, reorderLesson } from '../api/lessons';
import { CourseStatus } from '../types';
import type { Course, Lesson } from '../types';
import { Button } from '../components/Button';
import { Layout } from '../components/Layout';
import { Input } from '../components/Input';
import { ArrowLeft, Plus, Edit, Trash2, CheckCircle, XCircle } from 'lucide-react';

export const CourseDetails: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [course, setCourse] = useState<Course | null>(null);
    const [lessons, setLessons] = useState<Lesson[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isLessonModalOpen, setIsLessonModalOpen] = useState(false);
    const [editingLesson, setEditingLesson] = useState<Lesson | null>(null);
    const [lessonTitle, setLessonTitle] = useState('');

    const fetchData = async () => {
        if (!id) return;
        setIsLoading(true);
        try {
            const [courseData, lessonsData] = await Promise.all([
                getCourseSummary(id),
                getLessonsByCourse(id)
            ]);
            setCourse(courseData);
            // Ensure lessons are sorted by order
            setLessons(lessonsData.sort((a: Lesson, b: Lesson) => a.order - b.order));
        } catch (error) {
            console.error('Failed to fetch data', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, [id]);

    const handlePublishToggle = async () => {
        if (!course || !id) return;
        try {
            if (course.status === CourseStatus.Published) {
                await unpublishCourse(id);
            } else {
                await publishCourse(id);
            }
            fetchData();
        } catch (error) {
            console.error('Failed to update course status', error);
        }
    };

    const handleDeleteLesson = async (lessonId: string) => {
        if (window.confirm('Are you sure you want to delete this lesson?')) {
            try {
                await deleteLesson(lessonId);
                fetchData();
            } catch (error) {
                console.error('Failed to delete lesson', error);
            }
        }
    };

    const handleSaveLesson = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!id) return;

        try {
            if (editingLesson) {
                await updateLesson(editingLesson.id, {
                    title: lessonTitle,
                    order: editingLesson.order
                });
            } else {
                const newOrder = lessons.length > 0 ? Math.max(...lessons.map(l => l.order)) + 1 : 1;
                await createLesson({
                    courseId: id,
                    title: lessonTitle,
                    order: newOrder
                });
            }
            closeModal();
            fetchData();
        } catch (error) {
            console.error('Failed to save lesson', error);
        }
    };

    const openModal = (lesson?: Lesson) => {
        if (lesson) {
            setEditingLesson(lesson);
            setLessonTitle(lesson.title);
        } else {
            setEditingLesson(null);
            setLessonTitle('');
        }
        setIsLessonModalOpen(true);
    };

    const closeModal = () => {
        setIsLessonModalOpen(false);
        setEditingLesson(null);
        setLessonTitle('');
    };

    const handleReorder = async (lessonId: string, direction: 'up' | 'down') => {
        const currentIndex = lessons.findIndex(l => l.id === lessonId);
        if (currentIndex === -1) return;

        const newIndex = direction === 'up' ? currentIndex - 1 : currentIndex + 1;
        if (newIndex < 0 || newIndex >= lessons.length) return;

        const lessonToMove = lessons[currentIndex];
        const swapLesson = lessons[newIndex];

        // Optimistic update
        const newLessons = [...lessons];
        newLessons[currentIndex] = swapLesson;
        newLessons[newIndex] = lessonToMove;
        setLessons(newLessons);

        try {
            // Swap orders
            await reorderLesson(lessonToMove.id, swapLesson.order);
            await reorderLesson(swapLesson.id, lessonToMove.order);
            fetchData(); // Refresh to ensure sync
        } catch (error) {
            console.error('Failed to reorder', error);
            fetchData(); // Revert on error
        }
    };

    if (isLoading) return <Layout><div className="text-center py-10">Loading...</div></Layout>;
    if (!course) return <Layout><div className="text-center py-10">Course not found</div></Layout>;

    return (
        <Layout>
            <div className="max-w-4xl mx-auto">
                <div className="flex items-center justify-between mb-6">
                    <Button variant="outline" onClick={() => navigate('/')} className="flex items-center gap-2">
                        <ArrowLeft className="w-4 h-4" />
                        Back to Dashboard
                    </Button>

                    <div className="flex gap-2">
                        <Button
                            variant={course.status === CourseStatus.Published ? 'secondary' : 'primary'}
                            onClick={handlePublishToggle}
                            className="flex items-center gap-2"
                        >
                            {course.status === CourseStatus.Published ? (
                                <>
                                    <XCircle className="w-4 h-4" /> Unpublish
                                </>
                            ) : (
                                <>
                                    <CheckCircle className="w-4 h-4" /> Publish
                                </>
                            )}
                        </Button>
                        <Link to={`/courses/${course.id}/edit`}>
                            <Button variant="outline" className="flex items-center gap-2">
                                <Edit className="w-4 h-4" /> Edit Course
                            </Button>
                        </Link>
                    </div>
                </div>

                <div className="card p-6 mb-8">
                    <h1 className="text-3xl font-bold mb-2">{course.title}</h1>
                    <div className="flex items-center gap-2">
                        <span className={`text-sm px-2 py-1 rounded-full ${course.status === CourseStatus.Published
                            ? 'bg-green-100 text-green-800'
                            : 'bg-gray-100 text-gray-800'
                            }`}>
                            {course.status === CourseStatus.Published ? 'Published' : 'Draft'}
                        </span>
                    </div>
                </div>

                <div className="flex items-center justify-between mb-4">
                    <h2 className="text-xl font-bold">Lessons</h2>
                    <Button onClick={() => openModal()} className="flex items-center gap-2">
                        <Plus className="w-4 h-4" /> Add Lesson
                    </Button>
                </div>

                <div className="space-y-3">
                    {lessons.length === 0 ? (
                        <div className="text-center py-8 text-muted card bg-gray-50 border-dashed">
                            No lessons yet. Add your first lesson!
                        </div>
                    ) : (
                        lessons.map((lesson, index) => (
                            <div key={lesson.id} className="card p-4 flex items-center justify-between group">
                                <div className="flex items-center gap-4">
                                    <div className="flex flex-col gap-1 text-gray-400">
                                        <button
                                            disabled={index === 0}
                                            onClick={() => handleReorder(lesson.id, 'up')}
                                            className="hover:text-gray-700 disabled:opacity-30"
                                        >
                                            ▲
                                        </button>
                                        <button
                                            disabled={index === lessons.length - 1}
                                            onClick={() => handleReorder(lesson.id, 'down')}
                                            className="hover:text-gray-700 disabled:opacity-30"
                                        >
                                            ▼
                                        </button>
                                    </div>
                                    <span className="font-medium text-lg">{lesson.title}</span>
                                </div>

                                <div className="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                                    <Button variant="outline" className="p-2" onClick={() => openModal(lesson)}>
                                        <Edit className="w-4 h-4" />
                                    </Button>
                                    <Button
                                        variant="outline"
                                        className="p-2 text-red-500 hover:bg-red-50 border-red-200"
                                        onClick={() => handleDeleteLesson(lesson.id)}
                                    >
                                        <Trash2 className="w-4 h-4" />
                                    </Button>
                                </div>
                            </div>
                        ))
                    )}
                </div>
            </div>

            {/* Simple Modal for Lesson */}
            {isLessonModalOpen && (
                <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
                    <div className="bg-white rounded-lg p-6 w-full max-w-md shadow-xl">
                        <h2 className="text-xl font-bold mb-4">
                            {editingLesson ? 'Edit Lesson' : 'New Lesson'}
                        </h2>
                        <form onSubmit={handleSaveLesson}>
                            <Input
                                label="Lesson Title"
                                value={lessonTitle}
                                onChange={(e) => setLessonTitle(e.target.value)}
                                required
                                autoFocus
                            />
                            <div className="flex justify-end gap-3 mt-6">
                                <Button type="button" variant="outline" onClick={closeModal}>
                                    Cancel
                                </Button>
                                <Button type="submit">
                                    Save
                                </Button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </Layout>
    );
};
