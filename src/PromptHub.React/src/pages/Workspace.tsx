import React, { useState, useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import { motion, AnimatePresence } from 'framer-motion';
import { Rocket, Copy, Check, Sparkles, Cpu, Send, Loader2, AlertCircle, UserCircle } from 'lucide-react';
import { promptService, ExpandPromptRequest } from '../services/promptService';
import { RoleType } from '../types';

const PERSONAS = [
  { id: RoleType.GeneralAssistant, name: 'General Assistant', icon: '🤖' },
  { id: RoleType.SeniorDeveloper, name: 'Senior Developer', icon: '💻' },
  { id: RoleType.CreativeWriter, name: 'Creative Writer', icon: '✍️' },
  { id: RoleType.LegalExpert, name: 'Legal Expert', icon: '⚖️' },
  { id: RoleType.AcademicResearcher, name: 'Academic Researcher', icon: '🎓' },
  { id: RoleType.MarketingSpecialist, name: 'Marketing Specialist', icon: '📈' },
  { id: RoleType.DataAnalyst, name: 'Data Analyst', icon: '📊' },
];

const Workspace: React.FC = () => {
  const [searchParams] = useSearchParams();
  const [request, setRequest] = useState<ExpandPromptRequest>({
    originalInput: '',
    provider: 'Gemini',
    role: RoleType.GeneralAssistant,
    templateId: null
  });
  const [expandedOutput, setExpandedOutput] = useState('');
  const [isExpanding, setIsExpanding] = useState(false);
  const [isLoadingTemplate, setIsLoadingTemplate] = useState(false);
  const [copied, setCopied] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const templateId = searchParams.get('templateId');
    if (templateId) {
      loadTemplate(templateId);
    }
  }, [searchParams]);

  const loadTemplate = async (id: string) => {
    setIsLoadingTemplate(true);
    setError(null);
    try {
      const template = await promptService.getTemplateById(id);
      setRequest(prev => ({ 
        ...prev, 
        originalInput: template.content,
        templateId: id,
        role: template.defaultRole ?? prev.role
      }));
    } catch (err) {
      setError('Failed to load the selected template.');
    } finally {
      setIsLoadingTemplate(false);
    }
  };

  const handlePromptChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setRequest(prev => ({ ...prev, originalInput: e.target.value, templateId: null }));
  };

  const onExpand = async () => {
    if (!request.originalInput.trim()) return;
    setIsExpanding(true);
    setError(null);
    try {
      const response = await promptService.expandPrompt(request);
      setExpandedOutput(response.expandedPrompt);
    } catch (err) {
      console.error(err);
      setError('An error occurred during prompt expansion. Please check your connection and try again.');
    } finally {
      setIsExpanding(false);
    }
  };

  const copyToClipboard = () => {
    navigator.clipboard.writeText(expandedOutput);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      className="space-y-8"
    >
      <div className="flex flex-col gap-2">
        <h1 className="text-4xl font-bold tracking-tight">
          <span className="gradient-text">Workspace</span>
        </h1>
        <p className="text-[var(--text-muted)]">Select a persona and refine your prompts with high-precision AI expansion.</p>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-8 items-stretch">
        {/* Input Panel */}
        <motion.div 
          initial={{ x: -20, opacity: 0 }}
          animate={{ x: 0, opacity: 1 }}
          className="lg:col-span-7 premium-card pa-8 space-y-8"
        >
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-3">
              <Cpu className="text-[var(--primary)]" />
              <h2 className="text-xl font-bold">Configuration</h2>
            </div>
            {isLoadingTemplate && <Loader2 className="animate-spin text-[var(--primary)]" size={20} />}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="space-y-2">
              <label className="text-sm font-semibold text-[var(--text-muted)] uppercase tracking-wider flex items-center gap-2">
                <UserCircle size={16} /> AI Persona
              </label>
              <select
                value={request.role ?? 0}
                onChange={(e) => setRequest(prev => ({ ...prev, role: parseInt(e.target.value) }))}
                className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3 px-4 outline-none transition-all"
              >
                {PERSONAS.map(p => (
                  <option key={p.id} value={p.id}>{p.icon} {p.name}</option>
                ))}
              </select>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-semibold text-[var(--text-muted)] uppercase tracking-wider flex items-center gap-2">
                <Sparkles size={16} /> AI Provider
              </label>
              <select
                value={request.provider}
                onChange={(e) => setRequest(prev => ({ ...prev, provider: e.target.value }))}
                className="w-full bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl py-3 px-4 outline-none transition-all"
              >
                <option value="Gemini">Google Gemini Pro</option>
                <option value="OpenAI">OpenAI GPT-4</option>
                <option value="Anthropic">Anthropic Claude 3</option>
              </select>
            </div>
          </div>

          <div className="space-y-2">
            <label className="text-sm font-semibold text-[var(--text-muted)] uppercase tracking-wider">Raw Prompt Instruction</label>
            <textarea
              value={request.originalInput}
              onChange={handlePromptChange}
              placeholder="Enter your basic prompt here. The AI will expand it based on the selected persona."
              className="w-full h-64 bg-[var(--bg-dark)] border border-[var(--border-subtle)] focus:border-[var(--primary)] rounded-xl p-6 outline-none transition-all resize-none font-mono text-sm leading-relaxed"
            />
          </div>

          <div className="pt-4">
            <button
              onClick={onExpand}
              disabled={isExpanding || !request.originalInput.trim()}
              className="glow-btn w-full py-4 flex items-center justify-center gap-3 text-lg"
            >
              {isExpanding ? (
                <>
                  <Loader2 className="animate-spin" size={24} />
                  Expanding with AI...
                </>
              ) : (
                <>
                  <Rocket size={24} />
                  Expand High-Definition Prompt
                </>
              )}
            </button>
          </div>
        </motion.div>

        {/* Output Panel */}
        <motion.div 
          initial={{ x: 20, opacity: 0 }}
          animate={{ x: 0, opacity: 1 }}
          className="lg:col-span-5 glass-panel pa-8 flex flex-col h-full min-h-[500px]"
        >
          <div className="flex items-center justify-between mb-8">
            <div className="flex items-center gap-3 text-[var(--secondary)]">
              <Sparkles />
              <h2 className="text-xl font-bold">Refined Result</h2>
            </div>
            {expandedOutput && (
              <button
                onClick={copyToClipboard}
                className="p-2 hover:bg-white hover:bg-opacity-5 rounded-lg transition-colors flex items-center gap-2 text-sm"
              >
                {copied ? <Check size={18} className="text-green-400" /> : <Copy size={18} />}
                {copied ? 'Copied' : 'Copy'}
              </button>
            )}
          </div>

          <div className="flex-grow overflow-auto pr-2 custom-scrollbar">
            {isExpanding ? (
              <div className="h-full flex flex-col items-center justify-center gap-6 opacity-40">
                <motion.div
                  animate={{ scale: [1, 1.2, 1], opacity: [0.5, 1, 0.5] }}
                  transition={{ repeat: Infinity, duration: 2 }}
                >
                  <Send size={48} className="text-[var(--primary)]" />
                </motion.div>
                <p className="text-center font-medium">Communicating with AI...</p>
              </div>
            ) : expandedOutput ? (
              <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                className="whitespace-pre-wrap leading-loose text-lg text-[var(--text-main)] font-light"
              >
                {expandedOutput}
              </motion.div>
            ) : (
              <div className="h-full flex flex-col items-center justify-center text-center opacity-30 gap-4">
                <div className="p-6 rounded-full bg-white bg-opacity-5">
                  <Rocket size={48} />
                </div>
                <p className="text-lg">Your high-fidelity prompt will appear here after expansion.</p>
              </div>
            )}
          </div>

          <AnimatePresence>
            {error && (
              <motion.div
                initial={{ opacity: 0, y: 10 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0 }}
                className="mt-6 p-4 rounded-xl bg-red-500 bg-opacity-10 border border-red-500 border-opacity-20 flex items-start gap-3"
              >
                <AlertCircle className="text-red-400 shrink-0" size={20} />
                <p className="text-sm text-red-100">{error}</p>
              </motion.div>
            )}
          </AnimatePresence>
        </motion.div>
      </div>

      <style>{`
        .custom-scrollbar::-webkit-scrollbar { width: 6px; }
        .custom-scrollbar::-webkit-scrollbar-track { background: transparent; }
        .custom-scrollbar::-webkit-scrollbar-thumb { background: rgba(255,255,255,0.05); border-radius: 10px; }
        .custom-scrollbar::-webkit-scrollbar-thumb:hover { background: rgba(255,255,255,0.1); }
      `}</style>
    </motion.div>
  );
};

export default Workspace;
