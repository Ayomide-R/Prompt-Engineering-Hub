import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { UserPlus, Mail, Lock, User, ArrowRight, Loader2, AlertCircle, ShieldCheck } from 'lucide-react';
import { authService } from '../services/authService';
import { useAuth } from '../context/AuthContext';

const Register: React.FC = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (password !== confirmPassword) {
      setError('Passwords do not match.');
      return;
    }

    if (password.length < 8) {
      setError('Password must be at least 8 characters long.');
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      const response = await authService.register({ username, email, password });
      login(response);
      navigate('/workspace');
    } catch (err: any) {
      const backendError = err.response?.data?.errors;
      if (backendError) {
        // Handle specific FluentValidation errors
        const message = Object.values(backendError).flat()[0] as string;
        setError(message);
      } else {
        setError(err.response?.data?.message || 'Registration failed. Please check your details.');
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center p-4 py-20">
      <motion.div
        initial={{ y: 20, opacity: 0 }}
        animate={{ y: 0, opacity: 1 }}
        className="w-full max-w-md"
      >
        <div className="glass-panel pa-10 space-y-8 relative overflow-hidden group">
          <div className="absolute top-0 right-0 w-32 h-32 bg-[var(--primary-glow)] blur-3xl rounded-full opacity-20 group-hover:opacity-40 transition-opacity"></div>
          
          <div className="text-center space-y-2 relative">
            <motion.div
              initial={{ scale: 0.5, rotate: -10 }}
              animate={{ scale: 1, rotate: 0 }}
              className="inline-flex p-4 rounded-2xl bg-[var(--surface-dark)] border border-[var(--border-subtle)] text-[var(--primary)] mb-4 shadow-lg shadow-[var(--primary-glow)]"
            >
              <UserPlus size={32} />
            </motion.div>
            <h1 className="text-3xl font-extrabold tracking-tight">
              Create <span className="gradient-text">Account</span>
            </h1>
            <p className="text-[var(--text-muted)] font-medium">Join the future of prompt engineering.</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-5 relative">
            <div className="space-y-2">
              <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest pl-1">Username</label>
              <div className="relative group/input">
                <User className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within/input:text-[var(--primary)] transition-colors" size={20} />
                <input
                  type="text"
                  required
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3.5 pl-12 pr-4 outline-none transition-all focus:ring-4 focus:ring-[var(--primary-glow)]"
                  placeholder="ayomide"
                />
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest pl-1">Email Address</label>
              <div className="relative group/input">
                <Mail className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within/input:text-[var(--primary)] transition-colors" size={20} />
                <input
                  type="email"
                  required
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3.5 pl-12 pr-4 outline-none transition-all focus:ring-4 focus:ring-[var(--primary-glow)]"
                  placeholder="ayomide@prompthub.ai"
                />
              </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest pl-1">Password</label>
                <div className="relative group/input">
                  <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within/input:text-[var(--primary)] transition-colors" size={20} />
                  <input
                    type="password"
                    required
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3.5 pl-12 pr-4 outline-none transition-all focus:ring-4 focus:ring-[var(--primary-glow)]"
                    placeholder="••••••••"
                  />
                </div>
              </div>

              <div className="space-y-2">
                <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest pl-1">Confirm</label>
                <div className="relative group/input">
                  <ShieldCheck className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within/input:text-[var(--primary)] transition-colors" size={20} />
                  <input
                    type="password"
                    required
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3.5 pl-12 pr-4 outline-none transition-all focus:ring-4 focus:ring-[var(--primary-glow)]"
                    placeholder="••••••••"
                  />
                </div>
              </div>
            </div>

            {error && (
              <motion.div
                initial={{ opacity: 0, y: -10 }}
                animate={{ opacity: 1, y: 0 }}
                className="p-4 rounded-xl bg-red-500 bg-opacity-10 border border-red-500 border-opacity-20 flex items-start gap-3"
              >
                <AlertCircle className="text-red-400 shrink-0" size={20} />
                <p className="text-sm text-red-100">{error}</p>
              </motion.div>
            )}

            <motion.button
              whileHover={{ scale: 1.02 }}
              whileTap={{ scale: 0.98 }}
              disabled={isLoading}
              className="glow-btn w-full py-4 flex items-center justify-center gap-3 text-lg font-bold group/btn"
            >
              {isLoading ? (
                <Loader2 className="animate-spin" size={24} />
              ) : (
                <>
                  Register <ArrowRight className="group-hover/btn:translate-x-2 transition-transform" size={20} />
                </>
              )}
            </motion.button>
          </form>

          <div className="text-center pt-4 relative">
            <p className="text-[var(--text-muted)] text-sm">
              Already have an account?{' '}
              <Link to="/login" className="text-[var(--primary)] hover:underline font-bold">
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
