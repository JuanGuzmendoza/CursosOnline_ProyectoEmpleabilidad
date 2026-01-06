import api from './axios';
import type { LoginDto, RegisterDto } from '../types';

export const login = async (data: LoginDto) => {
    const response = await api.post('/Auth/login', data);
    return response.data;
};

export const register = async (data: RegisterDto) => {
    const response = await api.post('/Auth/register', data);
    return response.data;
};
