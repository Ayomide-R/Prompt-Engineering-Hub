import React from 'react';
import { motion } from 'framer-motion';
import { ExternalLink, Tag } from 'lucide-react';
import { PromptTemplateDto } from '../types';

interface TemplateCardProps {
  template: PromptTemplateDto;
  onUse: (id: string) => void;
}

const TemplateCard: React.FC<TemplateCardProps> = ({ template, onUse }) => {
  return (
    <motion.div
      layout
      className="h-full flex flex-col relative"
    >
      <div className="p-8 flex-grow flex flex-col relative z-10">
        <div className="flex items-center justify-between mb-6">
          <span className="px-4 py-1 bg-white/5 text-[var(--primary)] text-[10px] font-bold rounded-full border border-white/5 tracking-[0.2em] uppercase">
            {template.category}
          </span>
          <div className="w-8 h-8 rounded-lg bg-white/5 flex items-center justify-center opacity-0 group-hover:opacity-100 transition-all duration-500">
            <ExternalLink size={14} className="text-[var(--text-muted)]" />
          </div>
        </div>

        <h3 className="text-2xl font-bold mb-4 tracking-tight group-hover:gradient-text transition-all duration-500">
          {template.title}
        </h3>
        
        <p className="text-[var(--text-muted)] text-sm leading-relaxed line-clamp-4 mb-10 font-medium opacity-80">
          {template.content}
        </p>

        <div className="mt-auto">
          <button
            onClick={() => onUse(template.id)}
            className="w-full py-4 rounded-2xl bg-white/5 border border-white/10 text-white font-bold text-xs tracking-widest uppercase flex items-center justify-center gap-3 group-hover:bg-[var(--primary)] group-hover:border-[var(--primary)] transition-all duration-500"
          >
            Load into Workspace
            <motion.span animate={{ x: [0, 4, 0] }} transition={{ repeat: Infinity, duration: 2 }}>
              →
            </motion.span>
          </button>
        </div>
      </div>
      
      {/* Subtle Glow on Hover */}
      <div className="absolute inset-0 bg-gradient-to-br from-[var(--primary)] to-[var(--secondary)] opacity-0 group-hover:opacity-5 blur-3xl transition-opacity duration-700"></div>
    </motion.div>
  );
};

export default TemplateCard;
