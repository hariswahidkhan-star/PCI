/** "Skip to main content" — shared by both shells, and it has one hard requirement: it must be the
 *  FIRST focusable thing in the document. Placed after the sidebar it is worse than useless, because
 *  the reader has already tabbed through every nav link by the time it offers to skip them.
 *
 *  Focus is moved programmatically as well as by the fragment. The href alone works without
 *  scripting, but on its own it leaves `#main-content` in a BrowserRouter URL and some browsers move
 *  the scroll position without moving focus, so the next Tab returns to the navigation the reader
 *  just asked to leave. */
export default function SkipLink({ targetId = 'main-content' }: { targetId?: string }) {
  return (
    <a
      className="skip-link"
      href={`#${targetId}`}
      onClick={(e) => {
        const el = document.getElementById(targetId)
        if (!el) return // no target rendered yet: let the browser handle the fragment
        e.preventDefault()
        el.focus()
        el.scrollIntoView({ block: 'start' })
      }}
    >
      Skip to main content
    </a>
  )
}
