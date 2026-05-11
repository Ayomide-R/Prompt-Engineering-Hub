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
      initial={{ opacity: 0, scale: 0.98 }}
      animate={{ opacity: 1, scale: 1 }}
      className="space-y-12"
    >
      <div className="flex flex-col gap-4">
        <div className="flex items-center gap-4">
          <div className="h-px w-12 bg-[var(--primary)]"></div>
          <span className="text-sm font-bold tracking-[0.2em] text-[var(--primary)] uppercase">Neural Workspace</span>
        </div>
        <h1 className="text-5xl font-extrabold tracking-tighter">
          Refine your <span className="gradient-text">Neural Signals</span>
        </h1>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-10 items-stretch">
        {/* Input Panel */}
        <motion.div 
          initial={{ x: -30, opacity: 0 }}
          animate={{ x: 0, opacity: 1 }}
          className="lg:col-span-7 card-aura p-10 space-y-10"
        >
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-4">
              <div className="p-3 rounded-2xl bg-[var(--primary)] bg-opacity-10 border border-[var(--primary)] border-opacity-20">
                <Cpu className="text-[var(--primary)]" size={24} />
              </div>
              <h2 className="text-2xl font-bold tracking-tight">Configuration</h2>
            </div>
            {isLoadingTemplate && <Loader2 className="animate-spin text-[var(--primary)]" size={24} />}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
            <div className="space-y-3">
              <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest flex items-center gap-2">
                <UserCircle size={14} /> Intelligence Persona
              </label>
              <select
                value={request.role ?? 0}
                onChange={(e) => setRequest(prev => ({ ...prev, role: parseInt(e.target.value) }))}
                className="w-full input-cyber cursor-pointer appearance-none"
              >
                {PERSONAS.map(p => (
                  <option key={p.id} value={p.id} className="bg-[var(--surface)]">{p.icon} {p.name}</option>
                ))}
              </select>
            </div>

            <div className="space-y-3">
              <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest flex items-center gap-2">
                <Sparkles size={14} /> Compute Provider
              </label>
              <select
                value={request.provider}
                onChange={(e) => setRequest(prev => ({ ...prev, provider: e.target.value }))}
                className="w-full input-cyber cursor-pointer appearance-none"
              >
                <option value="Gemini" className="bg-[var(--surface)]">Google Gemini 1.5 Flash</option>
                <option value="OpenAI" className="bg-[var(--surface)]">OpenAI GPT-4 Turbo</option>
                <option value="Anthropic" className="bg-[var(--surface)]">Anthropic Claude 3.5</option>
              </select>
            </div>
          </div>

          <div className="space-y-3">
            <label className="text-xs font-bold text-[var(--text-muted)] uppercase tracking-widest">Base Instruction Set</label>
            <textarea
              value={request.originalInput}
              onChange={handlePromptChange}
              placeholder="Inject your core task here..."
              className="w-full h-80 input-cyber resize-none font-mono text-sm leading-relaxed"
            />
          </div>

          <button
            onClick={onExpand}
            disabled={isExpanding || !request.originalInput.trim()}
            className="btn-premium w-full text-xl py-5"
          >
            {isExpanding ? (
              <>
                <Loader2 className="animate-spin" size={28} />
                Synthesizing...
              </>
            ) : (
              <>
                <Rocket size={28} />
                Execute Neural Expansion
              </>
            )}
          </button>
        </motion.div>

        {/* Output Panel */}
        <motion.div 
          initial={{ x: 30, opacity: 0 }}
          animate={{ x: 0, opacity: 1 }}
          className="lg:col-span-5 card-aura p-10 flex flex-col h-full min-h-[600px] border-opacity-50"
          style={{ background: 'linear-gradient(180deg, var(--surface) 0%, rgba(15, 23, 42, 0.8) 100%)' }}
        >
          <div className="flex items-center justify-between mb-10">
            <div className="flex items-center gap-4">
              <div className="p-3 rounded-2xl bg-[var(--secondary)] bg-opacity-10 border border-[var(--secondary)] border-opacity-20">
                <Sparkles className="text-[var(--secondary)]" size={24} />
              </div>
              <h2 className="text-2xl font-bold tracking-tight">Optimized Result</h2>
            </div>
            {expandedOutput && (
              <button
                onClick={copyToClipboard}
                className="flex items-center gap-2 px-4 py-2 rounded-xl bg-white/5 hover:bg-white/10 border border-white/10 transition-all text-sm font-bold"
              >
                {copied ? <Check size={16} className="text-green-400" /> : <Copy size={16} />}
                {copied ? 'Copied' : 'Copy'}
              </button>
            )}
          </div>

          <div className="flex-grow overflow-auto pr-4 custom-scrollbar">
            {isExpanding ? (
              <div className="h-full flex flex-col items-center justify-center gap-8">
                <div className="relative">
                  <motion.div
                    animate={{ rotate: 360 }}
                    transition={{ repeat: Infinity, duration: 8, ease: "linear" }}
                    className="w-24 h-24 rounded-full border-2 border-dashed border-[var(--primary)] opacity-20"
                  />
                  <div className="absolute inset-0 flex items-center justify-center">
                    <Send size={40} className="text-[var(--primary)] animate-pulse" />
                  </div>
                </div>
                <p className="text-center font-bold tracking-widest text-[var(--text-muted)] uppercase text-xs">Processing via Neural Link...</p>
              </div>
            ) : expandedOutput ? (
              <motion.div
                initial={{ opacity: 0, y: 10 }}
                animate={{ opacity: 1, y: 0 }}
                className="whitespace-pre-wrap leading-relaxed text-[var(--text-main)] font-medium text-lg"
              >
                {expandedOutput}
              </motion.div>
            ) : (
              <div className="h-full flex flex-col items-center justify-center text-center opacity-20 gap-8">
                <div className="p-10 rounded-full border-2 border-dashed border-white/20">
                  <Rocket size={64} />
                </div>
                <p className="text-xl font-bold tracking-tight">Awaiting Neural Sequence...</p>
              </div>
            )}
          </div>

          <AnimatePresence>
            {error && (
              <motion.div
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0 }}
                className="mt-8 p-5 rounded-2xl bg-red-500/10 border border-red-500/20 flex items-start gap-4"
              >
                <AlertCircle className="text-red-400 shrink-0" size={24} />
                <div className="space-y-1">
                  <p className="font-bold text-red-400 text-sm">Communication Failure</p>
                  <p className="text-sm text-red-200/80 leading-relaxed">{error}</p>
                </div>
              </motion.div>
            )}
          </AnimatePresence>
        </motion.div>
      </div>
    </motion.div>
  );
};

export default Workspace;
