import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { motion } from 'framer-motion';
import { Search, Filter, Loader2 } from 'lucide-react';
import { promptService } from '../services/promptService';
import { PromptTemplateDto } from '../types';
import TemplateCard from '../components/TemplateCard';

const Home: React.FC = () => {
  const [templates, setTemplates] = useState<PromptTemplateDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchTemplates = async () => {
      try {
        const data = await promptService.getPublicTemplates();
        setTemplates(data);
      } catch (error) {
        console.error('Failed to fetch templates:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchTemplates();
  }, []);

  const filteredTemplates = templates.filter(t => 
    t.title.toLowerCase().includes(search.toLowerCase()) || 
    t.category.toLowerCase().includes(search.toLowerCase())
  );

  const container = {
    hidden: { opacity: 0 },
    show: {
      opacity: 1,
      transition: {
        staggerChildren: 0.1
      }
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      exit={{ opacity: 0, y: -20 }}
      className="space-y-12"
    >
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
        <div className="space-y-4">
          <h1 className="text-5xl font-extrabold tracking-tight">
            Discover <span className="gradient-text">Public Templates</span>
          </h1>
          <p className="text-[var(--text-muted)] text-lg max-w-2xl">
            Browse and discover optimized prompt templates crafted by the community to supercharge your AI workflows.
          </p>
        </div>

        <div className="relative group w-full md:w-96">
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within:text-[var(--primary)] transition-colors" size={20} />
          <input
            type="text"
            placeholder="Search templates or categories..."
            className="w-full bg-[var(--surface-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3 pl-12 pr-4 outline-none transition-all focus:ring-4 focus:ring-[var(--primary-glow)]"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
      </div>

      {loading ? (
        <div className="flex flex-col items-center justify-center py-24 gap-4">
          <Loader2 className="animate-spin text-[var(--primary)]" size={48} />
          <p className="text-[var(--text-muted)] animate-pulse">Syncing with registry...</p>
        </div>
      ) : filteredTemplates.length > 0 ? (
        <motion.div
          variants={container}
          initial="hidden"
          animate="show"
          className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8"
        >
          {filteredTemplates.map(template => (
            <TemplateCard 
              key={template.id} 
              template={template} 
              onUse={(id) => navigate(`/workspace?templateId=${id}`)} 
            />
          ))}
        </motion.div>
      ) : (
        <div className="glass-panel p-20 text-center flex flex-col items-center gap-6">
          <div className="w-20 h-20 bg-[var(--surface-dark)] rounded-2xl flex items-center justify-center border border-[var(--border-subtle)]">
            <Filter size={32} className="text-[var(--text-muted)]" />
          </div>
          <div className="space-y-2">
            <h3 className="text-2xl font-bold">No matches found</h3>
            <p className="text-[var(--text-muted)]">Try adjusting your filters or search terms.</p>
          </div>
          <button 
            onClick={() => setSearch('')}
            className="text-[var(--primary)] font-semibold hover:underline"
          >
            Clear all searches
          </button>
        </div>
      )}
    </motion.div>
  );
};

export default Home;
