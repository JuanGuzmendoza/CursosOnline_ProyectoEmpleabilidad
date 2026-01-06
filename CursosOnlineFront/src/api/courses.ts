import api from './axios';
import { CourseStatus } from '../types';
import type { CreateCourseDto, UpdateCourseDto } from '../types';

export const searchCourses = async (query?: string, status?: CourseStatus, page: number = 1, pageSize: number = 10) => {
    const params = { query, status, page, pageSize };
    const response = await api.get('/Courses/search', { params });
    return response.data;
};

export const getCourseSummary = async (id: string) => {
    const response = await api.get(`/Courses/${id}/summary`);
    return response.data;
};

export const createCourse = async (data: CreateCourseDto) => {
    const response = await api.post('/Courses', data);
    return response.data;
};

export const updateCourse = async (id: string, data: UpdateCourseDto) => {
    const response = await api.put(`/Courses/${id}`, data);
    return response.data;
};

export const deleteCourse = async (id: string) => {
    const response = await api.delete(`/Courses/${id}`);
    return response.data;
};

export const publishCourse = async (id: string) => {
    const response = await api.post(`/Courses/${id}/publish`);
    return response.data;
};

export const unpublishCourse = async (id: string) => {
    const response = await api.post(`/Courses/${id}/unpublish`);
    return response.data;
};
