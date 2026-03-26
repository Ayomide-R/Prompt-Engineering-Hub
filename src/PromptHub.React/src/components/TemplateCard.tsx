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
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      whileHover={{ y: -8 }}
      className="premium-card h-full flex flex-col group"
    >
      <div className="h-1 bg-gradient-to-r from-[var(--primary)] to-[var(--secondary)] opacity-0 group-hover:opacity-100 transition-opacity"></div>
      
      <div className="p-6 flex-grow flex flex-col">
        <div className="flex items-center justify-between mb-4">
          <span className="px-3 py-1 bg-[var(--primary-glow)] text-[var(--primary)] text-xs font-bold rounded-full flex items-center gap-1">
            <Tag size={12} />
            {template.category}
          </span>
          <button className="text-[var(--text-muted)] hover:text-white transition-colors">
            <ExternalLink size={16} />
          </button>
        </div>

        <h3 className="text-xl mb-3 pr-4 group-hover:text-[var(--primary)] transition-colors">
          {template.title}
        </h3>
        
        <p className="text-[var(--text-muted)] text-sm leading-relaxed line-clamp-3 mb-6">
          {template.content}
        </p>

        <div className="mt-auto">
          <button
            onClick={() => onUse(template.id)}
            className="text-[var(--primary)] font-semibold text-sm flex items-center gap-2 hover:gap-3 transition-all"
          >
            Use Template
            <motion.span animate={{ x: [0, 5, 0] }} transition={{ repeat: Infinity, duration: 1.5 }}>
              →
            </motion.span>
          </button>
        </div>
      </div>
    </motion.div>
  );
};

export default TemplateCard;
