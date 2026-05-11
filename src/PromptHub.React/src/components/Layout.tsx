import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { Sparkles, LayoutDashboard, Rocket, LogIn, LogOut, User } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { isAuthenticated, userEmail, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex flex-col bg-[var(--bg-dark)] text-[var(--text-main)] relative">
      {/* Background Mesh */}
      <div className="bg-mesh">
        <div className="mesh-circle" style={{ width: '600px', height: '600px', left: '-10%', top: '-10%', background: 'rgba(var(--primary-rgb), 0.2)' }}></div>
        <div className="mesh-circle" style={{ width: '500px', height: '500px', right: '-5%', top: '20%', background: 'rgba(var(--secondary-rgb), 0.2)', animationDelay: '-5s' }}></div>
      </div>

      {/* Navigation */}
      <div className="container-max pt-6">
        <nav className="glass-nav px-8 py-4 z-50 flex items-center justify-between">
          <Link to="/" className="flex items-center gap-3 no-underline group">
            <motion.div
              whileHover={{ rotate: 180, scale: 1.1 }}
              transition={{ type: 'spring', stiffness: 260, damping: 20 }}
              className="w-10 h-10 rounded-xl bg-gradient-to-br from-[var(--primary)] to-[var(--secondary)] flex items-center justify-center shadow-lg"
            >
              <Sparkles className="text-white" size={24} />
            </motion.div>
            <span className="gradient-text font-bold text-2xl tracking-tighter">PROMPT.HUB</span>
          </Link>
          
          <div className="flex gap-10 items-center">
            <Link to="/" className="flex items-center gap-2 text-[var(--text-muted)] hover:text-white transition-all font-semibold tracking-tight no-underline">
              <span>Discover</span>
            </Link>
            <Link to="/workspace" className="flex items-center gap-2 text-[var(--text-muted)] hover:text-white transition-all font-semibold tracking-tight no-underline">
              <span>Workspace</span>
            </Link>

            {isAuthenticated ? (
              <div className="flex items-center gap-6 pl-6 border-l border-white border-opacity-10">
                <div className="flex items-center gap-3 bg-white bg-opacity-5 py-2 px-4 rounded-full border border-white border-opacity-5">
                  <div className="w-6 h-6 rounded-full bg-gradient-to-r from-blue-500 to-purple-500"></div>
                  <span className="text-sm font-bold tracking-tight">{userEmail?.split('@')[0]}</span>
                </div>
                <button 
                  onClick={() => { logout(); navigate('/login'); }}
                  className="p-2 text-[var(--text-muted)] hover:text-white transition-colors"
                >
                  <LogOut size={20} />
                </button>
              </div>
            ) : (
              <Link to="/login" className="btn-premium py-2 px-6 text-sm no-underline">
                <LogIn size={18} />
                <span>Get Started</span>
              </Link>
            )}
          </div>
        </nav>
      </div>

      {/* Main Content */}
      <main className="flex-grow container-max py-12">
        {children}
      </main>

      {/* Footer */}
      <footer className="py-12 container-max text-center">
        <div className="h-px w-full bg-gradient-to-r from-transparent via-white/10 to-transparent mb-12"></div>
        <p className="text-[var(--text-muted)] text-sm font-medium tracking-wide opacity-60 uppercase">
          &copy; {new Date().getFullYear()} PromptHub Intelligence &bull; Architecture for the Neural Era
        </p>
      </footer>
    </div>
  );
};

export default Layout;
