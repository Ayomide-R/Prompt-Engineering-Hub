import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { motion } from 'framer-motion';
import { LogIn, Mail, Lock, Loader2, Sparkles, ArrowRight } from 'lucide-react';
import { authService } from '../services/authService';
import { useAuth } from '../context/AuthContext';

const Login: React.FC = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    
    try {
      const response = await authService.login({ email, password });
      login(response);
      navigate('/');
    } catch (err) {
      setError('Invalid email or password. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, scale: 0.95 }}
      animate={{ opacity: 1, scale: 1 }}
      className="min-h-[70vh] flex items-center justify-center p-4"
    >
      <div className="premium-card glass-panel w-full max-w-md pa-10 space-y-8 relative group">
        <div className="absolute -top-10 -left-10 w-32 h-32 bg-[var(--primary-glow)] blur-3xl rounded-full opacity-40"></div>
        <div className="absolute -bottom-10 -right-10 w-32 h-32 bg-[var(--secondary-glow)] blur-3xl rounded-full opacity-40"></div>

        <div className="text-center space-y-4">
          <div className="inline-flex items-center justify-center w-20 h-20 rounded-2xl bg-[var(--surface-dark)] border border-[var(--border-subtle)] group-hover:border-[var(--primary)] transition-colors">
            <Sparkles className="text-[var(--primary)]" size={40} />
          </div>
          <div className="space-y-2">
            <h1 className="text-3xl font-extrabold tracking-tight">
              Welcome <span className="gradient-text">Back</span>
            </h1>
            <p className="text-[var(--text-muted)]">Sign in to continue your creative journey.</p>
          </div>
        </div>

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="space-y-2">
            <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest pl-1">Email Address</label>
            <div className="relative">
              <Mail className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)]" size={18} />
              <input
                type="email"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="you@example.com"
                className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3.5 pl-12 pr-4 outline-none transition-all focus:ring-4 focus:ring-[var(--primary-glow)]"
              />
            </div>
          </div>

          <div className="space-y-2">
            <div className="flex justify-between items-end px-1">
              <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest">Password</label>
              <button type="button" className="text-xs text-[var(--primary)] hover:underline font-semibold">Forgot?</button>
            </div>
            <div className="relative">
              <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)]" size={18} />
              <input
                type="password"
                required
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
                className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3.5 pl-12 pr-4 outline-none transition-all focus:ring-4 focus:ring-[var(--primary-glow)]"
              />
            </div>
          </div>

          {error && (
            <motion.div 
              initial={{ opacity: 0, y: -10 }}
              animate={{ opacity: 1, y: 0 }}
              className="p-3 rounded-lg bg-red-500 bg-opacity-10 border border-red-500 border-opacity-20 text-red-100 text-sm text-center"
            >
              {error}
            </motion.div>
          )}

          <button
            type="submit"
            disabled={loading}
            className="glow-btn w-full py-4 flex items-center justify-center gap-3 text-lg group/btn"
          >
            {loading ? (
              <Loader2 className="animate-spin" size={24} />
            ) : (
              <>
                <LogIn size={20} />
                Sign In
                <ArrowRight className="group-hover/btn:translate-x-2 transition-transform" size={20} />
              </>
            )}
          </button>
        </form>

        <div className="text-center pt-2">
          <p className="text-[var(--text-muted)] text-sm">
            New to PromptHub? <Link to="/register" className="text-[var(--primary)] hover:underline font-bold">Create an account</Link>
          </p>
        </div>
      </div>
    </motion.div>
  );
};

export default Login;
