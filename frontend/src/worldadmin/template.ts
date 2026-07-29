// §10.2 share-template placeholder engine — a BOUNDED whitelist, escaping by construction.
//
// Admins author caption/title/hashtag templates as free text with `{placeholder}` slots. Only
// the two whitelisted placeholders are ever substituted; any other `{…}` token stays exactly as
// typed (visible to the author as "this did nothing"), so a template can never be a channel for
// smuggling values the backend never agreed to expose. The rendered result is a PLAIN STRING —
// previews put it in the DOM exclusively through React text nodes (never dangerouslySetInnerHTML),
// which is what makes a hostile display name inert. The test suite proves it with one.

export const PLACEHOLDER_WHITELIST = ['display_name', 'challenge_title'] as const
export type Placeholder = (typeof PLACEHOLDER_WHITELIST)[number]

/** Human labels for the placeholder chips in the editor. */
export const PLACEHOLDER_HELP: Record<Placeholder, string> = {
  display_name: 'The participant’s public display name (server-vouched).',
  challenge_title: 'The published challenge title being shared.',
}

const TOKEN = /\{([a-z0-9_]+)\}/g

function isWhitelisted(name: string): name is Placeholder {
  return (PLACEHOLDER_WHITELIST as readonly string[]).includes(name)
}

/** Substitute ONLY whitelisted placeholders; leave every other `{…}` token verbatim.
 *  Returns plain text — the caller must render it as a text node, never as HTML. */
export function renderTemplate(template: string, values: Record<Placeholder, string>): string {
  return template.replace(TOKEN, (whole, name: string) => (isWhitelisted(name) ? values[name] : whole))
}

/** Every `{…}` token in the template that is NOT on the whitelist — surfaced as an editor
 *  warning so authors learn the boundary instead of silently shipping a literal `{email}`. */
export function unknownPlaceholders(template: string): string[] {
  const out: string[] = []
  for (const m of template.matchAll(TOKEN)) {
    const name = m[1]
    if (!isWhitelisted(name) && !out.includes(name)) out.push(name)
  }
  return out
}
