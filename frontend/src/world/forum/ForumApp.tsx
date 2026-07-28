import { useCallback, useEffect, useRef, useState } from 'react'
import * as F from './api'

// PCI World forum — the authenticated COMPOSER surface (docs/pciworld/CCP_PHASE3_DESIGN.md §6).
//
// READING IS DELIBERATELY NOT HERE. Thread pages are server-rendered HTML with JSON-LD (D-009), so
// this component never fetches or renders a transcript. It is mounted by those pages — or stands
// alone for starting a discussion — and receives its context (thread id, the reader's own posts)
// as props from the page that owns it.
//
// THE SLICE'S SAFETY RULE SHOWS UP IN THE UI AS WELL: nothing posted here is ever echoed back as
// though it published. A held post is reported in the server's OWN words via its `notice` field,
// and a published one is confirmed with a link rather than a locally-rendered copy. The
// server-rendered page is the only place forum content appears, so this composer cannot disagree
// with it about what is visible — an optimistic echo is exactly the thing that would show a new
// member their held post as though everyone could read it.
//
// ACCESSIBILITY IS BUILT IN, NOT RETROFITTED (§18.1):
//   • Every outcome — held, published, refused — is conveyed in TEXT, never by colour alone.
//   • Outcomes land in a role="status" region: they are results, not emergencies, and an assertive
//     "alert" would interrupt a screen reader mid-word.
//   • Focus is never moved by anything the person did not just do. The only programmatic focus is
//     toward the field that needs fixing after THEIR OWN submit was refused.

type Notice = { tone: 'info' | 'held' | 'error'; text: string } | null

/** A post on the hosting page this composer may act on. Supplied by the server-rendered thread
 * page: `body` is the raw current revision for the author's own posts, so an edit starts from what
 * is actually there rather than an empty box. */
export interface PostRef {
  id: number
  mine: boolean
  body: string
}

export default function ForumApp({ threadId, threadTitle, posts = [] }: {
  threadId?: number
  threadTitle?: string
  posts?: PostRef[]
}) {
  const [me, setMe] = useState<F.Me | null>(null)
  const [categories, setCategories] = useState<F.Category[]>([])
  const [signedOut, setSignedOut] = useState(false)
  const [loading, setLoading] = useState(true)
  const [notice, setNotice] = useState<Notice>(null)

  const load = useCallback(async () => {
    setLoading(true)
    try {
      // /me first: an expired session should say "sign in" up front, not half-render a composer
      // whose first submit would fail after someone has already written their post.
      const who = await F.me()
      setMe(who)
      if (threadId === undefined) setCategories((await F.listCategories()).categories)
    } catch (e) {
      if (e instanceof F.ForumError && e.status === 401) setSignedOut(true)
      else setNotice({ tone: 'error', text: 'The forum is unavailable right now. Please try again shortly.' })
    } finally { setLoading(false) }
  }, [threadId])

  useEffect(() => { void load() }, [load])

  return (
    <div className="fw">
      <header className="fw-head">
        <h1>PCI World forum</h1>
      </header>

      {notice && (
        // role="status" rather than "alert": these are outcomes, not emergencies, and an assertive
        // region would interrupt a screen reader mid-word. The tone class is decoration on top of
        // words that already carry the whole meaning.
        <p role="status" className={`fw-notice fw-notice-${notice.tone}`}>{notice.text}</p>
      )}

      {loading && <p role="status">Loading…</p>}

      {signedOut && (
        <p className="fw-signin">
          Reading the forum needs no account. To post,
          please <a href="/world/">sign in to your PCI World account</a>.
        </p>
      )}

      {me?.posts_are_reviewed_first && (
        // Said BEFORE anyone writes anything, in the server's terms (GET /me), so a new member's
        // first post "vanishing" into review is expected rather than alarming.
        <p className="fw-reviewed">
          Your posts are reviewed by a moderator before other people can see them. Every newer
          account starts this way, and it lifts as your posts are accepted.
        </p>
      )}

      {me && threadId === undefined && (
        <ThreadComposer me={me} categories={categories} onNotice={setNotice} />
      )}

      {me && threadId !== undefined && (
        <ReplyComposer me={me} threadId={threadId} threadTitle={threadTitle} onNotice={setNotice} />
      )}

      {me && posts.length > 0 && <PostTools me={me} posts={posts} onNotice={setNotice} />}
    </div>
  )
}

// ── Starting a discussion ───────────────────────────────────────────────────────────────────

function ThreadComposer({ me, categories, onNotice }: {
  me: F.Me; categories: F.Category[]; onNotice: (n: Notice) => void
}) {
  const [category, setCategory] = useState('')
  const [title, setTitle] = useState('')
  const [body, setBody] = useState('')
  const [busy, setBusy] = useState(false)
  const [publishedUrl, setPublishedUrl] = useState<string | null>(null)
  const bodyRef = useRef<HTMLTextAreaElement>(null)

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    if (busy || !category) return
    setBusy(true); onNotice(null); setPublishedUrl(null)
    try {
      const r = await F.createThread(category, title.trim(), body)
      if (r.published) {
        onNotice({ tone: 'info', text: 'Your discussion is published.' })
        // The link is the confirmation. Only a PUBLISHED thread gets one — a held thread's page
        // does not exist for anyone yet, and linking to a 404 would contradict the notice.
        setPublishedUrl(r.url)
        setTitle(''); setBody('')
      } else {
        // Held. The server's own words, verbatim — and the form clears because the server is now
        // HOLDING the text: nothing is lost, and keeping the draft invites a duplicate.
        onNotice({ tone: 'held', text: r.notice ?? 'Your thread is waiting for a moderator.' })
        setTitle(''); setBody('')
      }
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof F.ForumError ? err.message : 'Your discussion could not be posted.' })
      // The draft is deliberately KEPT: a refusal must not also cost someone what they wrote.
      // Moving focus TO the field that needs fixing is helpful; moving it away from someone
      // mid-task is not — so this is the only programmatic focus in the composer.
      bodyRef.current?.focus()
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="fw-new-h" className="fw-composer">
      <h2 id="fw-new-h">Start a discussion</h2>

      <form onSubmit={submit} noValidate>
        <fieldset className="fw-cats">
          <legend>Category</legend>
          {categories.map(c => {
            const allowed = !c.read_only && F.atLeast(me.level, c.min_trust)
            const descId = c.description ? `fw-cat-d-${c.slug}` : null
            const whyId = allowed ? null : `fw-cat-why-${c.slug}`
            const describedBy = [descId, whyId].filter(Boolean).join(' ') || undefined
            return (
              <div key={c.slug} className="fw-cat">
                <label>
                  <input
                    type="radio" name="fw-cat" value={c.slug} disabled={!allowed}
                    checked={category === c.slug} onChange={() => setCategory(c.slug)}
                    aria-describedby={describedBy}
                  />
                  <span>{c.title}</span>
                </label>
                {c.description && <p id={descId ?? undefined} className="fw-help">{c.description}</p>}
                {!allowed && (
                  // The WHY, in words. A greyed-out control with no explanation reads as broken;
                  // "needs member standing" tells someone what would change it.
                  <p id={whyId ?? undefined} className="fw-help">
                    {c.read_only
                      ? 'Read only — new discussions cannot be started here.'
                      : `Needs ${c.min_trust} standing to post; your account is ${me.level}.`}
                  </p>
                )}
              </div>
            )
          })}
        </fieldset>

        <label htmlFor="fw-title">Title</label>
        <input
          id="fw-title" value={title} maxLength={200}
          onChange={e => setTitle(e.target.value)}
          aria-describedby="fw-title-help"
        />
        <p id="fw-title-help" className="fw-help">
          Between 8 and 200 characters. Say what the discussion is about, not just &ldquo;help&rdquo;.
        </p>

        <label htmlFor="fw-body">Your post</label>
        <textarea
          id="fw-body" ref={bodyRef} value={body} rows={6}
          onChange={e => setBody(e.target.value)}
          aria-describedby="fw-body-help"
        />
        <p id="fw-body-help" className="fw-help">
          {/* The link rule is told up front rather than discovered on refusal. */}
          {me.may_post_links
            ? 'Keep it professional. Edits are visible in the post history.'
            : 'Keep it professional. Links are not available to new accounts yet.'}
        </p>

        <button
          type="submit"
          disabled={busy || !category || title.trim().length < 8 || body.trim().length === 0}
        >
          {busy ? 'Posting…' : 'Post discussion'}
        </button>
      </form>

      {publishedUrl && (
        <p className="fw-published"><a href={publishedUrl}>View your discussion</a></p>
      )}
    </section>
  )
}

// ── Replying ────────────────────────────────────────────────────────────────────────────────

function ReplyComposer({ me, threadId, threadTitle, onNotice }: {
  me: F.Me; threadId: number; threadTitle?: string; onNotice: (n: Notice) => void
}) {
  const [body, setBody] = useState('')
  const [busy, setBusy] = useState(false)
  const bodyRef = useRef<HTMLTextAreaElement>(null)

  async function submit(e: React.FormEvent) {
    e.preventDefault()
    if (busy || body.trim().length === 0) return
    setBusy(true); onNotice(null)
    try {
      const r = await F.reply(threadId, body)
      if (r.published) {
        // No local echo even on success: the server-rendered page is the transcript, and a reload
        // shows the reply where it actually is. Confirmed in words instead.
        onNotice({ tone: 'info', text: 'Your reply is published. Reload the page to see it in the discussion.' })
        setBody('')
      } else {
        onNotice({ tone: 'held', text: r.notice ?? 'Your reply is waiting for a moderator.' })
        setBody('')
      }
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof F.ForumError ? err.message : 'Your reply could not be posted.' })
      // Refused, so the words stay — and focus goes to where the fixing happens.
      bodyRef.current?.focus()
    } finally { setBusy(false) }
  }

  return (
    <section aria-labelledby="fw-reply-h" className="fw-composer">
      <h2 id="fw-reply-h">{threadTitle ? `Reply — ${threadTitle}` : 'Reply to this discussion'}</h2>

      <form onSubmit={submit} noValidate>
        <label htmlFor="fw-reply">Your reply</label>
        <textarea
          id="fw-reply" ref={bodyRef} value={body} rows={5}
          onChange={e => setBody(e.target.value)}
          aria-describedby="fw-reply-help"
        />
        <p id="fw-reply-help" className="fw-help">
          {me.may_post_links
            ? 'Keep it professional. Edits are visible in the post history.'
            : 'Keep it professional. Links are not available to new accounts yet.'}
        </p>
        <button type="submit" disabled={busy || body.trim().length === 0}>
          {busy ? 'Posting…' : 'Post reply'}
        </button>
      </form>
    </section>
  )
}

// ── Acting on existing posts (edit / delete / flag) ─────────────────────────────────────────

function PostTools({ me, posts, onNotice }: {
  me: F.Me; posts: PostRef[]; onNotice: (n: Notice) => void
}) {
  return (
    <section aria-labelledby="fw-tools-h" className="fw-tools">
      <h2 id="fw-tools-h">Post actions</h2>
      <ul className="fw-posts">
        {posts.map(p => <PostRow key={p.id} me={me} post={p} onNotice={onNotice} />)}
      </ul>
    </section>
  )
}

function PostRow({ me, post, onNotice }: {
  me: F.Me; post: PostRef; onNotice: (n: Notice) => void
}) {
  const [mode, setMode] = useState<'idle' | 'edit' | 'flag' | 'confirm_delete'>('idle')
  const [editBody, setEditBody] = useState(post.body)
  const [editReason, setEditReason] = useState('')
  const [flagReason, setFlagReason] = useState('')
  const [busy, setBusy] = useState(false)
  const [removed, setRemoved] = useState(false)

  // Once deleted the actions must go with it — offering "edit" on a hidden post would imply it is
  // still there to edit.
  if (removed) return <li className="fw-post">Post {post.id} is no longer visible.</li>

  async function saveEdit(e: React.FormEvent) {
    e.preventDefault()
    if (busy || editBody.trim().length === 0) return
    setBusy(true); onNotice(null)
    try {
      const r = await F.editPost(post.id, editBody, editReason.trim() || undefined)
      // The edit result carries no server notice, only state — so the words are chosen here, and
      // chosen to keep a held edit unmistakably distinct from a published one.
      onNotice(r.published
        ? { tone: 'info', text: 'Your edit is published. The previous version stays in the post history.' }
        : r.state === 'pending'
          ? { tone: 'held', text: 'Your edited post is waiting for a moderator. It is not visible to others until it is approved.' }
          : { tone: 'held', text: 'Your edit was not published.' })
      setMode('idle')
    } catch (err) {
      // The edit form stays open with the text intact: a refusal must not cost the rewrite.
      onNotice({ tone: 'error', text: err instanceof F.ForumError ? err.message : 'Your edit could not be saved.' })
    } finally { setBusy(false) }
  }

  async function doDelete() {
    if (busy) return
    setBusy(true); onNotice(null)
    try {
      const r = await F.deletePost(post.id)
      // The server's wording, verbatim — it is careful to say "hidden, record kept", and
      // paraphrasing it to "deleted" would promise an erasure that did not happen.
      onNotice({ tone: 'info', text: r.notice })
      setRemoved(true)
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof F.ForumError ? err.message : 'That post could not be deleted.' })
      setMode('idle')
    } finally { setBusy(false) }
  }

  async function sendFlag(e: React.FormEvent) {
    e.preventDefault()
    if (busy || flagReason.trim().length === 0) return
    setBusy(true); onNotice(null)
    try {
      const r = await F.flagPost(post.id, flagReason.trim())
      onNotice({ tone: 'info', text: r.notice ?? 'You have already flagged this post.' })
      setMode('idle'); setFlagReason('')
    } catch (err) {
      onNotice({ tone: 'error', text: err instanceof F.ForumError ? err.message : 'That flag could not be sent.' })
    } finally { setBusy(false) }
  }

  return (
    <li className="fw-post">
      {/* Capabilities come from GET /me, so a control that would only ever 403 is not offered —
          but its absence is explained by the review-first / standing text above, not left silent. */}
      <div className="fw-post-actions">
        {post.mine && me.may_edit && (
          <button type="button" onClick={() => setMode('edit')} disabled={busy}>
            Edit post {post.id}
          </button>
        )}
        {post.mine && (
          <button type="button" onClick={() => setMode('confirm_delete')} disabled={busy}>
            Delete post {post.id}
          </button>
        )}
        {!post.mine && me.may_flag && (
          <button type="button" onClick={() => setMode('flag')} disabled={busy}>
            Flag post {post.id}
          </button>
        )}
      </div>

      {mode === 'edit' && (
        <form onSubmit={saveEdit} noValidate className="fw-edit">
          <label htmlFor={`fw-edit-${post.id}`}>New text for post {post.id}</label>
          <textarea
            id={`fw-edit-${post.id}`} value={editBody} rows={5}
            onChange={e => setEditBody(e.target.value)}
            aria-describedby={`fw-edit-help-${post.id}`}
          />
          <label htmlFor={`fw-edit-why-${post.id}`}>Reason for the edit (optional)</label>
          <input
            id={`fw-edit-why-${post.id}`} value={editReason} maxLength={200}
            onChange={e => setEditReason(e.target.value)}
          />
          <p id={`fw-edit-help-${post.id}`} className="fw-help">
            Edits are visible: the previous version stays in the post&rsquo;s history.
          </p>
          <button type="submit" disabled={busy || editBody.trim().length === 0}>Save edit</button>
          <button type="button" onClick={() => setMode('idle')}>Cancel</button>
        </form>
      )}

      {mode === 'confirm_delete' && (
        <div className="fw-confirm">
          {/* The consequence is stated BEFORE the click, in the same terms the server will use
              after it — hiding, not erasing. */}
          <p>Deleting hides the post from everyone. Its history is kept while any report or appeal is open.</p>
          <button type="button" onClick={() => void doDelete()} disabled={busy}>Yes, delete it</button>
          <button type="button" onClick={() => setMode('idle')}>Keep it</button>
        </div>
      )}

      {mode === 'flag' && (
        <form onSubmit={sendFlag} noValidate className="fw-flag">
          <label htmlFor={`fw-flag-${post.id}`}>Why should a moderator look at post {post.id}?</label>
          <input
            id={`fw-flag-${post.id}`} value={flagReason} maxLength={300}
            onChange={e => setFlagReason(e.target.value)}
            aria-describedby={`fw-flag-help-${post.id}`}
          />
          <p id={`fw-flag-help-${post.id}`} className="fw-help">
            A flag asks a moderator to look. It does not remove the post by itself.
          </p>
          <button type="submit" disabled={busy || flagReason.trim().length === 0}>Send flag</button>
          <button type="button" onClick={() => setMode('idle')}>Cancel</button>
        </form>
      )}
    </li>
  )
}
