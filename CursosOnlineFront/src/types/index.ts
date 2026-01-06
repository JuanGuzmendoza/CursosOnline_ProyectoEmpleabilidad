export interface LoginDto {
    email?: string | null;
    password?: string | null;
}

export interface RegisterDto {
    email?: string | null;
    password?: string | null;
    firstName?: string | null;
    lastName?: string | null;
}

export interface CreateCourseDto {
    title: string;
}

export interface UpdateCourseDto {
    title: string;
}

export interface CreateLessonDto {
    courseId: string;
    title: string;
    order: number;
}

export interface UpdateLessonDto {
    title: string;
    order: number;
}

export const CourseStatus = {
    Draft: 0,
    Published: 1
} as const;

export type CourseStatus = typeof CourseStatus[keyof typeof CourseStatus];

export interface Course {
    id: string;
    title: string;
    status: CourseStatus;
    // Add other properties if returned by API, based on common patterns or assume minimal for now
}

export interface Lesson {
    id: string;
    courseId: string;
    title: string;
    order: number;
    // Add other properties if returned by API
}

export interface User {
    email: string;
    firstName?: string;
    lastName?: string;
    token?: string; // Assuming token might be stored here or handled separately
}
