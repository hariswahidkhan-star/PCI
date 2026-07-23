import { readFileSync } from 'node:fs'
import path from 'node:path'

interface DemoQuestion { question: string; answer_index: number }

export const demoQuestions = (JSON.parse(
  readFileSync(path.resolve(process.cwd(), '../backend/demo_exam_seed.json'), 'utf8'),
) as { questions: DemoQuestion[] }).questions

// schema.sql ships six founding PCL-AI live items before the opt-in demo pack is loaded. Keep their
// answer mapping beside the pack loader so a fresh end-to-end sitting answers the complete bank.
const foundingQuestions: DemoQuestion[] = [
  { question: 'Which metric best indicates cost efficiency in earned value management?', answer_index: 1 },
  { question: 'When applying AI to schedule risk analysis, the professional’s primary responsibility is to:', answer_index: 1 },
  { question: 'A schedule performance index (SPI) of 0.85 indicates the project is:', answer_index: 2 },
  { question: 'Contingency reserves are best sized using:', answer_index: 1 },
  { question: 'The primary purpose of a cost baseline is to:', answer_index: 1 },
  { question: 'Under PCI’s governed-AI principle, “AI proposes, the professional disposes” means:', answer_index: 1 },
]

export const demoAnswers = new Map([...foundingQuestions, ...demoQuestions].map((q) => [q.question, q.answer_index]))

export function sourceDemoQuestion(question: string): string {
  return question.replace(/^(PFL-AI|PML-AI): /, '')
}
