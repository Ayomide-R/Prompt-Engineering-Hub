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
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      className="space-y-20"
    >
      <section className="flex flex-col items-center text-center gap-8 py-10">
        <motion.div
          initial={{ opacity: 0, scale: 0.9 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.5 }}
          className="px-4 py-1.5 rounded-full bg-white/5 border border-white/10 text-sm font-semibold tracking-wider text-[var(--primary)] uppercase"
        >
          Elevate Your AI Interactions
        </motion.div>
        <h1 className="text-6xl md:text-8xl font-extrabold tracking-tighter leading-[0.9]">
          Master the <br />
          <span className="gradient-text">Art of Prompts</span>
        </h1>
        <p className="text-[var(--text-muted)] text-xl max-w-3xl leading-relaxed font-medium">
          The world's most advanced registry for high-fidelity AI prompt engineering. 
          Discover, refine, and deploy optimized templates for the next generation of LLMs.
        </p>
        
        <div className="w-full max-w-2xl mt-8">
          <div className="relative group">
            <Search className="absolute left-6 top-1/2 -translate-y-1/2 text-[var(--text-muted)] group-focus-within:text-[var(--primary)] transition-colors" size={24} />
            <input
              type="text"
              placeholder="Search the neural registry..."
              className="w-full input-cyber py-5 pl-16 pr-6 text-lg"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
        </div>
      </section>

      <div className="space-y-8">
        <div className="flex items-center justify-between">
          <h2 className="text-3xl font-bold tracking-tight flex items-center gap-3">
            <Sparkles className="text-[var(--primary)]" />
            Curated Templates
          </h2>
          <div className="h-px flex-grow mx-8 bg-gradient-to-r from-white/10 to-transparent"></div>
        </div>

        {loading ? (
          <div className="flex flex-col items-center justify-center py-24 gap-6">
            <div className="relative">
              <Loader2 className="animate-spin text-[var(--primary)]" size={64} />
              <div className="absolute inset-0 blur-xl bg-[var(--primary)] opacity-20"></div>
            </div>
            <p className="text-[var(--text-muted)] font-bold tracking-widest uppercase text-sm animate-pulse">Synchronizing Registry...</p>
          </div>
        ) : filteredTemplates.length > 0 ? (
          <motion.div
            variants={container}
            initial="hidden"
            animate="show"
            className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-10"
          >
            {filteredTemplates.map(template => (
              <div key={template.id} className="card-aura group">
                <TemplateCard 
                  template={template} 
                  onUse={(id) => navigate(`/workspace?templateId=${id}`)} 
                />
              </div>
            ))}
          </motion.div>
        ) : (
          <div className="card-aura p-24 text-center flex flex-col items-center gap-8">
            <div className="w-24 h-24 bg-white/5 rounded-3xl flex items-center justify-center border border-white/10">
              <Filter size={40} className="text-[var(--text-muted)]" />
            </div>
            <div className="space-y-3">
              <h3 className="text-3xl font-bold">Registry Entry Not Found</h3>
              <p className="text-[var(--text-muted)] text-lg">No templates match your current neural filters.</p>
            </div>
            <button 
              onClick={() => setSearch('')}
              className="btn-premium px-8"
            >
              Reset Filters
            </button>
          </div>
        )}
      </div>
    </motion.div>
  );
};

export default Home;
