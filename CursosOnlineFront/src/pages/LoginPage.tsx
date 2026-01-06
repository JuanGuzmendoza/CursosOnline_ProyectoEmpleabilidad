import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { login as loginApi } from '../api/auth';
import { Button } from '../components/Button';
import { Input } from '../components/Input';
import { Layout } from '../components/Layout';

export const LoginPage: React.FC = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setIsLoading(true);

        try {
            const data = await loginApi({ email, password });

            // Extraemos el token y usuario de la respuesta de la API
            const token = data.accessToken;
            const user = data.user;

            if (!token) throw new Error('No se recibió token del servidor');

            // Guardamos token y usuario en el contexto
            login(token, user);

            // Mostrar token en consola y alert (solo para pruebas)
            console.log('Token recibido:', token);
            alert(`Token recibido:\n${token}`);

            navigate('/');
        } catch (err: any) {
            setError('Correo o contraseña inválidos');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <Layout>
            <div className="max-w-md mx-auto mt-10">
                <div className="card p-6">
                    <h1 className="text-2xl font-bold text-center mb-6">Welcome Back</h1>

                    {error && (
                        <div className="bg-red-50 text-red-500 p-3 rounded mb-4 text-sm">
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit}>
                        <Input
                            label="Email"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            placeholder="john@example.com"
                        />
                        <Input
                            label="Password"
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                            placeholder="••••••••"
                        />

                        <Button type="submit" className="w-full mt-2" isLoading={isLoading}>
                            Login
                        </Button>
                    </form>

                    <p className="text-center mt-4 text-sm text-muted">
                        Don't have an account?{' '}
                        <Link to="/register" className="text-accent hover:underline">
                            Register
                        </Link>
                    </p>
                </div>
            </div>
        </Layout>
    );
};
