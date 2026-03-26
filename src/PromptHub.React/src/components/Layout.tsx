import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { Sparkles, LayoutDashboard, Rocket, LogIn, LogOut, User } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const { isAuthenticated, userEmail, logout } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex flex-col bg-[var(--bg-dark)] text-[var(--text-main)]">
      {/* Navigation */}
      <nav className="glass-panel sticky top-4 mx-4 my-2 px-8 py-4 z-50 flex items-center justify-between">
        <Link to="/" className="flex items-center gap-2 no-underline group">
          <motion.div
            whileHover={{ rotate: 180, scale: 1.2 }}
            transition={{ type: 'spring', stiffness: 200 }}
          >
            <Sparkles className="text-[var(--primary)]" size={32} />
          </motion.div>
          <span className="gradient-text font-bold text-2xl tracking-tight">PromptHub</span>
        </Link>
        
        <div className="flex gap-8 items-center">
          <Link to="/" className="flex items-center gap-2 text-[var(--text-muted)] hover:text-white transition-colors group">
            <LayoutDashboard size={20} className="group-hover:scale-110 transition-transform" />
            <span className="font-medium">Discover</span>
          </Link>
          <Link to="/workspace" className="flex items-center gap-2 text-[var(--text-muted)] hover:text-white transition-colors group">
            <Rocket size={20} className="group-hover:scale-110 transition-transform" />
            <span className="font-medium">Workspace</span>
          </Link>

          {isAuthenticated ? (
            <div className="flex items-center gap-4 pl-4 border-l border-white border-opacity-10">
              <div className="flex items-center gap-2 text-[var(--text-muted)]">
                <User size={18} />
                <span className="text-sm font-medium">{userEmail}</span>
              </div>
              <button 
                onClick={() => { logout(); navigate('/login'); }}
                className="text-[var(--text-muted)] hover:text-white transition-colors"
                title="Logout"
              >
                <LogOut size={20} />
              </button>
            </div>
          ) : (
            <Link to="/login" className="glow-btn flex items-center gap-2 no-underline">
              <LogIn size={20} />
              <span>Sign In</span>
            </Link>
          )}
        </div>
      </nav>

      {/* Hero Section Background Glow */}
      <div className="fixed top-0 left-1/2 -translate-x-1/2 w-[1000px] h-[600px] bg-[var(--primary-glow)] blur-[120px] -z-10 rounded-full opacity-50 pointer-events-none"></div>

      {/* Main Content */}
      <main className="flex-grow p-8 max-w-7xl mx-auto w-full">
        {children}
      </main>

      {/* Footer */}
      <footer className="p-8 text-center text-[var(--text-muted)] text-sm border-t border-white border-opacity-5 mt-12">
        <p>&copy; {new Date().getFullYear()} PromptHub. Crafted for the future of AI engineering.</p>
      </footer>
    </div>
  );
};

export default Layout;
