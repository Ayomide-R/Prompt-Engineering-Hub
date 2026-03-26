import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { UserPlus, Mail, Lock, User, ArrowRight, Loader2, AlertCircle } from 'lucide-react';
import { authService } from '../services/authService';
import { useAuth } from '../context/AuthContext';

const Register: React.FC = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    try {
      const response = await authService.register({ username, email, password });
      login(response);
      navigate('/workspace');
    } catch (err: any) {
      setError(err.response?.data?.errors?.Password?.[0] || 'Registration failed. Please check your details.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center p-4">
      <motion.div
        initial={{ y: 20, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        className="w-full max-w-md"
      >
        <div className="glass-panel pa-10 space-y-8">
          <div className="text-center space-y-2">
            <motion.div
              initial={{ scale: 0.5 }}
              animate={{ scale: 1 }}
              className="inline-flex p-4 rounded-2xl bg-[var(--primary-glow)] text-[var(--primary)] mb-4"
            >
              <UserPlus size={32} />
            </motion.div>
            <h1 className="text-3xl font-bold tracking-tight">Create Account</h1>
            <p className="text-[var(--text-muted)]">Join the future of prompt engineering.</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-5">
            <div className="space-y-2">
              <label className="text-sm font-medium text-[var(--text-muted)]">Username</label>
              <div className="relative group">
                <User className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within:text-[var(--primary)] transition-colors" size={20} />
                <input
                  type="text"
                  required
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3 pl-12 pr-4 outline-none transition-all"
                  placeholder="ayomide"
                />
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium text-[var(--text-muted)]">Email Address</label>
              <div className="relative group">
                <Mail className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within:text-[var(--primary)] transition-colors" size={20} />
                <input
                  type="email"
                  required
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3 pl-12 pr-4 outline-none transition-all"
                  placeholder="ayomide@prompthub.ai"
                />
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium text-[var(--text-muted)]">Password</label>
              <div className="relative group">
                <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within:text-[var(--primary)] transition-colors" size={20} />
                <input
                  type="password"
                  required
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3 pl-12 pr-4 outline-none transition-all"
                  placeholder="••••••••"
                />
              </div>
            </div>

            <motion.button
              whileHover={{ scale: 1.02 }}
              whileTap={{ scale: 0.98 }}
              disabled={isLoading}
              className="glow-btn w-full py-4 flex items-center justify-center gap-3 text-lg font-bold"
            >
              {isLoading ? (
                <Loader2 className="animate-spin" size={24} />
              ) : (
                <>
                  Register <ArrowRight size={20} />
                </>
              )}
            </motion.button>

            {error && (
              <motion.div
                initial={{ opacity: 0, scale: 0.95 }}
                animate={{ opacity: 1, scale: 1 }}
                className="p-4 rounded-xl bg-red-500 bg-opacity-10 border border-red-500 border-opacity-20 flex items-start gap-3"
              >
                <AlertCircle className="text-red-400 shrink-0" size={20} />
                <p className="text-sm text-red-100">{error}</p>
              </motion.div>
            )}
          </form>

          <div className="text-center pt-4">
            <p className="text-[var(--text-muted)]">
              Already have an account?{' '}
              <Link to="/login" className="text-[var(--primary)] hover:underline font-semibold">
                Sign In
              </Link>
            </p>
          </div>
        </div>
      </motion.div>
    </div>
  );
};

export default Register;
