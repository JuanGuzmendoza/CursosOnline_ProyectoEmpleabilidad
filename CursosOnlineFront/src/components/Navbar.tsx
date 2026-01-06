import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { LogOut, BookOpen } from 'lucide-react';
import { Button } from './Button';

export const Navbar: React.FC = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <nav className="navbar">
            <div className="container flex items-center justify-between h-16">
                <Link to="/" className="flex items-center gap-2 font-bold text-xl">
                    <BookOpen className="w-6 h-6 text-accent" />
                    <span>CoursesOnline</span>
                </Link>

                <div className="flex items-center gap-4">
                    {user ? (
                        <>
                            <span className="text-sm font-medium hidden sm:block">
                                {user.firstName} {user.lastName}
                            </span>
                            <Button variant="outline" onClick={handleLogout} className="flex items-center gap-2">
                                <LogOut className="w-4 h-4" />
                                <span className="hidden sm:inline">Logout</span>
                            </Button>
                        </>
                    ) : (
                        <>
                            <Link to="/login">
                                <Button variant="outline">Login</Button>
                            </Link>
                            <Link to="/register">
                                <Button>Register</Button>
                            </Link>
                        </>
                    )}
                </div>
            </div>
        </nav>
    );
};
