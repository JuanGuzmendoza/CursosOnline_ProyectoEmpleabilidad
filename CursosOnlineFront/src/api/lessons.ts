import api from './axios';
import type { CreateLessonDto, UpdateLessonDto } from '../types';

export const getLessonsByCourse = async (courseId: string) => {
    const response = await api.get(`/Lessons/course/${courseId}`);
    return response.data;
};

export const createLesson = async (data: CreateLessonDto) => {
    const response = await api.post('/Lessons', data);
    return response.data;
};

export const updateLesson = async (id: string, data: UpdateLessonDto) => {
    const response = await api.put(`/Lessons/${id}`, data);
    return response.data;
};

export const deleteLesson = async (id: string) => {
    const response = await api.delete(`/Lessons/${id}`);
    return response.data;
};

export const reorderLesson = async (id: string, newOrder: number) => {
    const response = await api.patch(`/Lessons/${id}/reorder`, null, {
        params: { newOrder }
    });
    return response.data;
};
