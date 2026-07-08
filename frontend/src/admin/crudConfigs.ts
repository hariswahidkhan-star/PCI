import type { CrudConfig } from './CrudSection'

// Generic content collections, each backed by the server's uniform /api/admin/{name} CRUD factory
// (backend/Endpoints/AdminMgmt.cs). Columns mirror those registered there; `perm` is the RBAC section.

export const QUESTION_BANK: CrudConfig = {
  name: 'sample_questions',
  path: 'questions',
  title: 'Question bank',
  perm: 'sampleq',
  description: 'Exam and practice questions.',
  addLabel: 'Add question',
  fields: [
    { key: 'question', label: 'Question', type: 'textarea', inList: true },
    { key: 'option_a', label: 'Option A' },
    { key: 'option_b', label: 'Option B' },
    { key: 'option_c', label: 'Option C' },
    { key: 'option_d', label: 'Option D' },
    { key: 'answer_index', label: 'Correct option (0=A,1=B,2=C,3=D)', type: 'number', inList: true },
    { key: 'domain', label: 'Domain', inList: true },
    { key: 'certification_id', label: 'Certification ID', type: 'number' },
    { key: 'is_practice', label: 'Practice question', type: 'checkbox' },
    { key: 'published', label: 'Published', type: 'checkbox', inList: true },
    { key: 'sort_order', label: 'Sort order', type: 'number' },
  ],
}

export const MEDIA: CrudConfig = {
  name: 'media_assets',
  path: 'media',
  title: 'Media library',
  perm: 'media',
  description: 'Registered media assets and their metadata.',
  addLabel: 'Add asset',
  fields: [
    { key: 'filename', label: 'Filename', inList: true },
    { key: 'alt', label: 'Alt text', inList: true },
    { key: 'width', label: 'Width', type: 'number' },
    { key: 'height', label: 'Height', type: 'number' },
    { key: 'usage', label: 'Usage', inList: true },
  ],
}

export const FAQS: CrudConfig = {
  name: 'faqs',
  path: 'faqs',
  title: 'FAQs',
  perm: 'faqs',
  description: 'Frequently asked questions shown on the website.',
  addLabel: 'Add FAQ',
  fields: [
    { key: 'question', label: 'Question', inList: true },
    { key: 'answer', label: 'Answer', type: 'textarea' },
    { key: 'category', label: 'Category', inList: true },
    { key: 'sort_order', label: 'Sort order', type: 'number' },
    { key: 'published', label: 'Published', type: 'checkbox', inList: true },
  ],
}

export const RESOURCES: CrudConfig = {
  name: 'resources',
  path: 'resources',
  title: 'Resources',
  perm: 'resources',
  description: 'Downloads and documents available to students.',
  addLabel: 'Add resource',
  fields: [
    { key: 'title', label: 'Title', inList: true },
    { key: 'category', label: 'Category', inList: true },
    { key: 'doc_type', label: 'Type' },
    { key: 'url', label: 'URL', inList: true },
    { key: 'published', label: 'Published', type: 'checkbox', inList: true },
    { key: 'sort_order', label: 'Sort order', type: 'number' },
  ],
}

export const NEWS: CrudConfig = {
  name: 'news',
  path: 'news',
  title: 'News',
  perm: 'news',
  description: 'Announcements and news items.',
  addLabel: 'Add news item',
  fields: [
    { key: 'title', label: 'Title', inList: true },
    { key: 'body', label: 'Body', type: 'textarea' },
    { key: 'published_date', label: 'Published date (YYYY-MM-DD)', inList: true },
    { key: 'published', label: 'Published', type: 'checkbox', inList: true },
    { key: 'sort_order', label: 'Sort order', type: 'number' },
  ],
}

export const BOK: CrudConfig = {
  name: 'bok_domains',
  path: 'bok',
  title: 'Body of Knowledge',
  perm: 'bok',
  description: 'Knowledge domains and their exam weightings.',
  addLabel: 'Add domain',
  fields: [
    { key: 'code', label: 'Code', inList: true },
    { key: 'name', label: 'Name', inList: true },
    { key: 'weight', label: 'Weight (%)', type: 'number', inList: true },
    { key: 'description', label: 'Description', type: 'textarea' },
    { key: 'sort_order', label: 'Sort order', type: 'number' },
  ],
}

export const GOVERNANCE: CrudConfig = {
  name: 'governance_roles',
  path: 'governance',
  title: 'Governance',
  perm: 'governance',
  description: 'Governance roles and their holders.',
  addLabel: 'Add role',
  fields: [
    { key: 'role', label: 'Role', inList: true },
    { key: 'holder', label: 'Holder', inList: true },
    { key: 'status', label: 'Status', inList: true },
    { key: 'remit', label: 'Remit', type: 'textarea' },
    { key: 'sort_order', label: 'Sort order', type: 'number' },
  ],
}

export const NAV: CrudConfig = {
  name: 'nav_items',
  path: 'navigation',
  title: 'Navigation menus',
  perm: 'nav',
  description: 'Header and footer navigation links.',
  addLabel: 'Add link',
  fields: [
    { key: 'label', label: 'Label', inList: true },
    { key: 'url', label: 'URL', inList: true },
    { key: 'nav_group', label: 'Menu group', inList: true },
    { key: 'sort_order', label: 'Sort order', type: 'number' },
    { key: 'visible', label: 'Visible', type: 'checkbox', inList: true },
  ],
}

export const CRUD_SECTIONS: CrudConfig[] = [QUESTION_BANK, MEDIA, FAQS, RESOURCES, NEWS, BOK, GOVERNANCE, NAV]
