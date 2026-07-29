/* Every surface is rendered from this one table so the chrome, the gate strip and the honesty
   footnote can never drift apart between screens. `gates` is the part that matters: it states what
   the real server enforces, next to a mock that cannot enforce anything. */
const S = {

/* ══════════════════════════ COMMUNITY ══════════════════════════ */
rooms: {
  kicker: 'Community', title: 'Rooms', path: '/world-app/community',
  gates: ['Open to <b>guests</b> — no account needed', 'Text is reviewed <b>before</b> anyone sees it',
          'With no moderation provider configured, nothing publishes at all'],
  lede: `Short-lived topical rooms for people who do this work. A guest picks a display name and
         joins — no sign-up wall, because a sign-up wall defeats the point of a guest room.`,
  html: `
  <div class="grid2">
    ${[['Planning Clinic','Bring a scheduling problem. No sales, no recruitment.',14,'open'],
       ['Cost & EVM','Earned value in practice — measurement, not reporting theatre.',9,'open'],
       ['Risk Hour','Quantitative risk, drawdown and contingency.',0,'scheduled'],
       ['Tender Room','Bid strategy and estimating under time pressure.',6,'open']]
      .map(([t,d,n,st]) => `
      <div class="card2">
        <h2>${t} ${st==='open' ? '<span class="pill live">Open</span>' : '<span class="pill gilt">Opens 14:00</span>'}</h2>
        <p class="muted" style="margin:0 0 12px">${d}</p>
        <p class="muted num" style="margin:0 0 14px">${n} in the room</p>
        <button class="btn" ${st==='open'?'':'disabled'} data-go="room">${st==='open'?'Join as a guest':'Not open yet'}</button>
      </div>`).join('')}
  </div>
  <div class="notice held"><b>Read-only today.</b> No moderation provider is contracted, so the room
    opens but no guest message can publish. That is the intended failure direction — silence rather
    than unreviewed text — not an outage.</div>`
},

room: {
  kicker: 'Community', title: 'Inside a room', path: '/world-app/community/planning-clinic',
  gates: ['Your own message shows as <b>pending</b> until the server allows it',
          'Nobody else sees it in the meantime', 'An image is held to the same rule'],
  lede: `The one behaviour worth looking at closely: your own message appears immediately, marked
         pending, and reaches nobody until the verdict comes back. It is never shown to another
         person first and withdrawn afterwards.`,
  html: `
  <div class="mock">
    <div class="mock-bar"><i></i><i></i><i></i><span class="mock-url">pci-world / rooms / planning-clinic</span></div>
    <div class="mock-body">
      <h2>Planning Clinic <span class="pill live">Open</span></h2>
      <p class="muted" style="margin:0 0 18px">Bring a scheduling problem. No sales, no recruitment.</p>
      <ul class="chat">
        <li><span class="who">Ama</span><p class="msg">Four-week slip on a shutdown. Client wants the
          date held. What do you actually do first?</p></li>
        <li><span class="who">Tomasz</span><p class="msg">Find out whether the driving path is really
          driving. Half the recoveries I've seen compressed the wrong chain.</p></li>
        <li><span class="who">Ama</span><p class="msg">It's the tie-ins. Everything else has float.</p></li>
        <li class="pending"><span class="who">You</span><p class="msg">Then the honest answer is the
          date moves or the scope does — compressing tie-ins buys you rework, not time.
          <span class="pill held" style="margin-left:6px">Pending review</span></p></li>
      </ul>
      <div class="notice held" role="status">Your message is with the reviewer. It is not visible to
        anyone else yet, and you will see it appear here if it is allowed.</div>
      <label for="rm">Your message</label>
      <textarea id="rm" placeholder="Type here…"></textarea>
      <p class="btnrow" style="margin-top:12px"><button class="btn">Send</button>
        <button class="btn ghost">Attach an image</button>
        <span class="help">Images are reviewed the same way, before anyone can load them.</span></p>
    </div>
  </div>`
},

/* ══════════════════════════ FORUM ══════════════════════════ */
forum: {
  kicker: 'Forum', title: 'Discussions', path: '/world/forum',
  gates: ['Reading needs <b>no account</b>', 'Server-rendered, so search engines can read it',
          'Only published threads appear — held ones are absent, not greyed out'],
  lede: `The public archive. This index did not exist until recently — thread pages were reachable
         only by someone who already had the link, which made the whole forum invisible.`,
  html: `
  <div class="mock">
    <div class="mock-bar"><i></i><i></i><i></i><span class="mock-url">/world/forum</span></div>
    <div class="mock-nav"><span class="wm">PCI World</span><a href="#">Today's Challenge</a>
      <a href="#">Challenge Library</a><a href="#">Writing</a><a href="#" class="new">Rooms</a>
      <a href="#" class="on">Forum</a><a href="#" class="new">Jobs</a><a href="#">Passport</a></div>
    <div class="mock-body">
      <h1>PCI World forum</h1>
      <p class="muted" style="margin:0 0 22px">Project controls practice, discussed in the open.
        Reading needs no account.</p>
      ${[['Estimating &amp; Forecasting','Cost and schedule estimating in practice.',
          ['How do you model escalation on a five-year job',
           'Bottom-up vs parametric when the scope is still moving',
           'What does your estimate basis document actually contain?']],
         ['Risk &amp; Contingency','Quantitative risk analysis, drawdown and contingency.',
          ['Drawdown curves that survive contact with a steering group',
           'P50 or P80 — and who decides?']],
         ['Planning &amp; Scheduling','Programme structure, logic and float discipline.',
          []]]
        .map(([t,d,items]) => `
        <div class="card2">
          <h2 style="font-family:var(--serif);font-weight:400;font-size:20px">${t}</h2>
          <p class="muted" style="margin:0 0 10px">${d}</p>
          ${items.length ? `<ul style="margin:0;padding-left:20px">${items.map(i=>
            `<li style="margin:5px 0"><a href="#" data-go="thread">${i}</a></li>`).join('')}</ul>`
            : `<p class="muted" style="margin:0"><em>No discussions here yet.</em></p>`}
        </div>`).join('')}
    </div>
  </div>
  <div class="foot"><b>Why an empty category says so in words.</b> Rendering nothing would look like
    a broken page. Saying "no discussions here yet" is a fact about the forum, not a rendering
    failure the reader has to diagnose.</div>`
},

thread: {
  kicker: 'Forum', title: 'A thread', path: '/world/forum/estimating/escalation-on-a-five-year-job',
  gates: ['Every post here has been <b>published</b> — held posts are not on the page at all',
          'Carries <code>DiscussionForumPosting</code> structured data',
          'Reflows to a 320px screen without sideways scrolling'],
  lede: `A discussion as a reader and a crawler see it. What reaches this page is the thing that
         matters most in the whole feature — search engines cache it, so anything that escapes onto
         it can outlive the deletion meant to fix it.`,
  html: `
  <div class="mock">
    <div class="mock-bar"><i></i><i></i><i></i><span class="mock-url">/world/forum/estimating/escalation-on-a-five-year-job</span></div>
    <div class="mock-body">
      <p class="crumbs"><a href="#">Forum</a> / <a href="#" data-go="forum">Estimating &amp; Forecasting</a></p>
      <h1>How do you model escalation on a five-year job</h1>
      <ul class="rows" style="margin-top:18px">
        <li>
          <p class="muted" style="margin:0 0 6px"><b style="color:var(--ink)">Dana Okoro</b> · 3 days ago</p>
          <p style="margin:0">We stopped using a single CPI series and moved to a blended index,
            rebaselining quarterly. The blended view separated packages that were genuinely late from
            those that were mis-estimated at sanction — which one index had been hiding. Curious what
            others do on long-duration work where the commodity mix shifts.</p>
        </li>
        <li style="background:var(--live-bg);margin:0 -12px;padding:14px 12px;border-radius:5px">
          <p style="margin:0 0 6px"><span class="pill live">Marked as the answer</span></p>
          <p class="muted" style="margin:0 0 6px"><b style="color:var(--ink)">Priya Raman</b> · 2 days ago</p>
          <p style="margin:0">Blend by commodity weight, not by spend. We hold three series — labour,
            steel, and a general index — and reweight at each rebaseline against the remaining
            quantities rather than the remaining budget. The difference shows up around year three,
            when the mix has moved but the budget split hasn't.</p>
        </li>
        <li>
          <p class="muted" style="margin:0 0 6px"><b style="color:var(--ink)">Tomasz Nowak</b> · yesterday</p>
          <p style="margin:0">Worth adding: whatever you choose, write down why in the estimate basis.
            The next planner inherits your index and no reasoning.</p>
          <p class="muted" style="margin:8px 0 0"><em>Edited to add the second sentence.</em></p>
        </li>
      </ul>
    </div>
  </div>`
},

composer: {
  kicker: 'Forum', title: 'Composer', path: '/world-app/ → Start a discussion',
  gates: ['Posting needs a <b>Passport account</b>', 'A new account\'s first posts are held for review',
          'The outcome is shown in the server\'s own words, never a paraphrase'],
  lede: `Where someone writes. The screen states up front that new accounts are reviewed first, so a
         first post "vanishing" into a queue is expected rather than alarming.`,
  html: `
  <div class="card2" style="max-width:720px">
    <div class="notice held" style="margin-top:0">Your posts are reviewed by a moderator before other
      people can see them. Every newer account starts this way, and it lifts as your posts are accepted.</div>
    <h2>Start a discussion</h2>
    <fieldset style="border:1px solid var(--line);border-radius:6px;padding:12px 14px;margin:14px 0">
      <legend style="font:600 13px var(--sans);color:var(--slate);padding:0 6px">Category</legend>
      <p style="margin:6px 0"><label style="display:flex;gap:8px;align-items:baseline;margin:0">
        <input type="radio" name="c" checked><span>Estimating &amp; Forecasting</span></label></p>
      <p style="margin:6px 0"><label style="display:flex;gap:8px;align-items:baseline;margin:0">
        <input type="radio" name="c"><span>Risk &amp; Contingency</span></label></p>
      <p style="margin:6px 0"><label style="display:flex;gap:8px;align-items:baseline;margin:0;opacity:.6">
        <input type="radio" name="c" disabled><span>Institute Announcements</span></label>
        <span class="help" style="margin-left:26px">Needs staff standing to post; your account is new.</span></p>
    </fieldset>
    <label for="t">Title</label>
    <input id="t" type="text" value="Rebaselining cadence on multi-year frameworks">
    <p class="help">Between 8 and 200 characters. Say what the discussion is about, not just "help".</p>
    <label for="b">Your post</label>
    <textarea id="b">Quarterly has worked for us, but I've seen monthly on shorter jobs. What drives the choice where you are?</textarea>
    <p class="help">New accounts cannot include links yet.</p>
    <p class="btnrow" style="margin-top:16px"><button class="btn" data-demo="hold">Post discussion</button></p>
    <div id="holdout"></div>
  </div>`,
  wire(root) {
    root.querySelector('[data-demo=hold]').addEventListener('click', () => {
      root.querySelector('#holdout').innerHTML =
        `<div class="notice held" role="status">Your thread is waiting for a moderator. New accounts
         are reviewed before publication.</div>
         <p class="help">The draft is cleared because the server is now holding the text — nothing is
         lost, and keeping a copy here invites a duplicate.</p>`;
      root.querySelector('#t').value = ''; root.querySelector('#b').value = '';
    });
  }
},

/* ══════════════════════════ JOBS ══════════════════════════ */
jobs: {
  kicker: 'Jobs', title: 'Job board', path: '/world/careers',
  gates: ['Only <b>verified</b> employers appear', 'A closed job leaves this list the moment it closes',
          'Applying always needs a Passport account'],
  lede: `Roles posted by employers PCI has checked. Verification is a person's judgement about who is
         behind a posting — a fake employer here is not a spammer, it is a CV-harvesting attack.`,
  html: `
  <div class="mock">
    <div class="mock-bar"><i></i><i></i><i></i><span class="mock-url">/world/careers</span></div>
    <div class="mock-nav"><span class="wm">PCI World</span><a href="#">Today's Challenge</a>
      <a href="#">Challenge Library</a><a href="#">Writing</a><a href="#" class="new">Rooms</a>
      <a href="#" class="new">Forum</a><a href="#" class="on">Jobs</a><a href="#">Passport</a></div>
    <div class="mock-body">
      <h1>Careers</h1>
      <p class="muted" style="margin:0 0 22px">Roles posted by verified employers. Applications are
        made in PCI World, with your consent recorded.</p>
      <ul class="rows">
        ${[['Senior Planning Engineer','Northwind Infrastructure','Manchester, UK','£65,000 – £78,000'],
           ['Cost Control Lead','Northwind Infrastructure','Remote (UK)','£58,000 – £70,000'],
           ['Project Controls Manager','Halden Energy','Aberdeen, UK','£82,000 – £95,000'],
           ['Planner (Rail)','Meridian Delivery Partners','Birmingham, UK','£48,000 – £56,000']]
          .map(([t,e,l,s]) => `
          <li>
            <p style="margin:0 0 4px;font:400 19px/1.3 var(--serif)"><a href="#" data-go="job">${t}</a></p>
            <p class="muted" style="margin:0">${e} · ${l} · <span class="num">${s}</span>
              <span class="pill gilt" style="margin-left:6px">Verified employer</span></p>
          </li>`).join('')}
      </ul>
    </div>
  </div>`
},

job: {
  kicker: 'Jobs', title: 'A job page', path: '/world/careers/northwind/senior-planning-engineer',
  gates: ['Carries <code>JobPosting</code> structured data <b>only while it is open</b>',
          'Once closed: page stays readable, markup disappears, leaves the sitemap',
          'No external apply link — ever'],
  lede: `A crawlable job page. Structured data is a claim made to a search engine, so it is computed
         when the page is drawn rather than left behind by a job that has since closed.`,
  html: `
  <div class="mock">
    <div class="mock-bar"><i></i><i></i><i></i><span class="mock-url">/world/careers/northwind/senior-planning-engineer</span></div>
    <div class="mock-body">
      <p class="crumbs"><a href="#" data-go="jobs">Careers</a> / <a href="#">Northwind Infrastructure</a></p>
      <h1>Senior Planning Engineer</h1>
      <p class="muted" style="margin:0 0 4px"><b style="color:var(--ink)">Northwind Infrastructure</b>
        <span class="pill gilt" style="margin-left:6px">Verified employer</span></p>
      <p class="muted num" style="margin:0 0 20px">Manchester, UK · Full time · £65,000 – £78,000 ·
        Closes 14 August</p>
      <p>Own the integrated programme for a live rail package: baseline maintenance, critical path
        analysis, and the monthly forecast that the client actually reads.</p>
      <h2 style="margin-top:22px">What you'll be asked when you apply</h2>
      <ul style="margin:0 0 22px"><li>Describe a schedule recovery you personally owned.</li></ul>
      <div class="card2" style="border-color:var(--gilt)">
        <h3>Apply</h3>
        <p class="muted" style="margin:0 0 12px">Your application, your answers and your CV are sent
          to this employer and to nobody else.</p>
        <p style="margin:0 0 10px"><label style="display:flex;gap:8px;align-items:flex-start;margin:0">
          <input type="checkbox" id="cons" style="margin-top:4px">
          <span style="font:14.5px/1.5 var(--sans);color:var(--ink)">I consent to Northwind
            Infrastructure receiving this application and the CV attached to it, for recruiting for
            this role. <span class="muted">(Policy v1 — you can withdraw this later.)</span></span>
        </label></p>
        <p class="btnrow"><button class="btn" data-demo="apply">Send application</button>
          <span class="help">Applying twice to one role is impossible by design.</span></p>
        <div id="applyout"></div>
      </div>
    </div>
  </div>`,
  wire(root) {
    root.querySelector('[data-demo=apply]').addEventListener('click', () => {
      const ok = root.querySelector('#cons').checked;
      root.querySelector('#applyout').innerHTML = ok
        ? `<div class="notice live" role="status">Your application has been sent. What you wrote, the
           CV you attached and the consent you gave are recorded as they were at this moment and will
           not change.</div>`
        : `<div class="notice stop" role="status">Tick the consent box to apply. Without it there is
           nothing to send — the employer only ever receives what you have agreed to disclose.</div>`;
    });
  }
},

employer: {
  kicker: 'Jobs', title: 'Employer portal', path: '/world-app/ → Employer portal',
  gates: ['An <b>unverified</b> employer can draft everything and publish nothing',
          'One employer can never see another\'s applicants',
          'Suspending an employer cuts its access mid-session, not at next login'],
  lede: `The employer's side. Note the second organisation: it can write postings all day and the
         publish control refuses, in words, with the reason.`,
  html: `
  <div class="card2">
    <h2>Your organisations</h2>
    <div class="btnrow">
      <button class="btn">Northwind Infrastructure</button>
      <span class="muted">state: <b>verified</b> · your role: owner</span>
    </div>
    <div class="btnrow" style="margin-top:10px">
      <button class="btn ghost">Halden Energy</button>
      <span class="muted">state: <b>pending verification</b> · your role: owner</span>
    </div>
  </div>

  <div class="card2">
    <h2>Postings — Northwind Infrastructure</h2>
    <div class="scroll"><table>
      <thead><tr><th>Role</th><th>State</th><th>Applicants</th><th></th></tr></thead>
      <tbody>
        <tr><td>Senior Planning Engineer</td><td><span class="pill live">Published</span></td>
          <td class="num">7</td><td><button class="btn ghost" data-demo="close">Close</button></td></tr>
        <tr><td>Cost Control Lead</td><td><span class="pill live">Published</span></td>
          <td class="num">3</td><td><button class="btn ghost">Close</button></td></tr>
        <tr><td>Contracts Engineer</td><td><span class="pill held">Draft</span></td>
          <td class="num">—</td><td><button class="btn">Publish</button></td></tr>
      </tbody>
    </table></div>
    <div id="closeout"></div>
  </div>

  <div class="card2" style="border-color:var(--held-ink)">
    <h2>Postings — Halden Energy</h2>
    <div class="scroll"><table>
      <thead><tr><th>Role</th><th>State</th><th></th></tr></thead>
      <tbody><tr><td>Project Controls Manager</td><td><span class="pill held">Draft</span></td>
        <td><button class="btn" data-demo="nopub">Publish</button></td></tr></tbody>
    </table></div>
    <div id="nopubout"></div>
  </div>

  <div class="card2">
    <h2>Applicants — Senior Planning Engineer</h2>
    <div class="scroll"><table>
      <thead><tr><th>Applicant</th><th>Submitted</th><th>CV</th><th>State</th><th></th></tr></thead>
      <tbody>
        <tr><td>A. Mensah</td><td>2 days ago</td><td><a href="#">Download</a></td>
          <td><span class="pill live">Shortlisted</span></td><td><button class="btn ghost">Open</button></td></tr>
        <tr><td>J. Whitfield</td><td>3 days ago</td><td><a href="#">Download</a></td>
          <td><span class="pill held">Submitted</span></td><td><button class="btn ghost">Open</button></td></tr>
        <tr><td>R. Okonkwo</td><td>4 days ago</td>
          <td class="muted">Unavailable</td><td><span class="pill stop">Consent withdrawn</span></td>
          <td><button class="btn ghost" disabled>Open</button></td></tr>
      </tbody>
    </table></div>
    <p class="help" style="margin-top:10px">Every CV download is logged — including refused ones,
      because a run of refusals is what probing looks like.</p>
  </div>`,
  wire(root) {
    root.querySelector('[data-demo=nopub]').addEventListener('click', () => {
      root.querySelector('#nopubout').innerHTML =
        `<div class="notice stop" role="status">Only a verified organisation can publish a job. Your
         draft is saved and stays private until verification completes.</div>`;
    });
    root.querySelector('[data-demo=close]').addEventListener('click', () => {
      root.querySelector('#closeout').innerHTML =
        `<div class="notice held" role="status">Closed. The page stays readable for anyone holding the
         link, but it no longer tells search engines the job is open and it has left the sitemap.</div>`;
    });
  }
},

apps: {
  kicker: 'Jobs', title: 'My applications', path: '/world-app/ → My job applications',
  gates: ['Withdrawing consent stops the employer immediately',
          'The consent record <b>survives</b> withdrawal', 'Withdrawal is not erasure — and says so'],
  lede: `The applicant's own view. The design decision worth seeing: withdrawing consent does not
         delete the record that consent existed, because "was there consent the day they read this?"
         has to stay answerable afterwards.`,
  html: `
  <ul class="rows">
    <li class="card2" style="margin-bottom:14px">
      <p style="margin:0 0 4px;font:400 19px var(--serif)">Senior Planning Engineer</p>
      <p class="muted" style="margin:0 0 10px">Northwind Infrastructure · submitted 3 days ago ·
        <span class="pill live">Shortlisted</span></p>
      <p class="muted" style="margin:0 0 10px">CV attached: <b>a-mensah-cv.pdf</b> · Consent granted
        3 days ago under policy v1 — <b style="color:var(--live-ink)">active</b></p>
      <p class="btnrow"><button class="btn ghost" data-demo="wd">Withdraw consent</button>
        <button class="btn ghost">Withdraw application</button></p>
      <div id="wdout"></div>
    </li>
    <li class="card2" style="margin-bottom:0">
      <p style="margin:0 0 4px;font:400 19px var(--serif)">Planner (Rail)</p>
      <p class="muted" style="margin:0 0 10px">Meridian Delivery Partners · submitted 3 weeks ago ·
        <span class="pill stop">Not progressing</span></p>
      <p class="muted" style="margin:0">Consent granted 3 weeks ago under policy v1 —
        <b>withdrawn</b> 2 days ago. The record is kept.</p>
    </li>
  </ul>`,
  wire(root) {
    root.querySelector('[data-demo=wd]').addEventListener('click', () => {
      root.querySelector('#wdout').innerHTML =
        `<div class="notice held" role="status">Consent withdrawn. The employer can no longer open this
         application or your CV. The record that you applied, and that you consented at the time, is
         kept — withdrawing consent is not the same as erasure.</div>`;
    });
  }
},

/* ══════════════════════════ PUBLISHING ══════════════════════════ */
contrib: {
  kicker: 'Publishing', title: 'Contributor desk', path: 'proposed — /world-app/ → Writing desk',
  gates: ['<b>No screen exists for this today</b> — the engine is built, the UI is not',
          'Contributor standing is granted by a person, never earned automatically',
          'A published article is never edited in place'],
  lede: `This is the one surface with no interface at all: the publishing engine, its permissions and
         its maker-checker were built and tested, and nothing was ever drawn. So this screen is a
         design proposal rather than a picture of something that exists.`,
  html: `
  <div class="card2">
    <h2>Your standing</h2>
    <p style="margin:0 0 8px"><span class="pill live">Contributor</span>
      <span class="muted" style="margin-left:8px">Granted 4 March by the editorial team.</span></p>
    <p class="muted" style="margin:0">You may write and submit. Publication is decided by an editor
      who is not you.</p>
  </div>

  <div class="card2">
    <h2>Your manuscripts</h2>
    <div class="scroll"><table>
      <thead><tr><th>Title</th><th>State</th><th>With</th><th></th></tr></thead>
      <tbody>
        <tr><td>What a good estimate basis actually contains</td>
          <td><span class="pill live">Published</span></td><td class="muted">—</td>
          <td><button class="btn ghost">View</button></td></tr>
        <tr><td>Recovering a slipped programme without lying about the date</td>
          <td><span class="pill held">In review</span></td><td>Editor: R. Vance</td>
          <td><button class="btn ghost">Open thread</button></td></tr>
        <tr><td>Contingency drawdown for people who hate spreadsheets</td>
          <td><span class="pill gilt">Drafting</span></td><td class="muted">—</td>
          <td><button class="btn">Continue</button></td></tr>
      </tbody>
    </table></div>
  </div>

  <div class="card2">
    <h2>Submit “Recovering a slipped programme”</h2>
    <p class="muted" style="margin:0 0 4px">Before an editor reads it, four questions. These are
      recorded as you answer them now and are not editable afterwards.</p>
    <label>Do you have a commercial interest in anything named in this piece?</label>
    <select><option>No</option><option>Yes — described below</option></select>
    <label>Was any part of this sponsored or paid for?</label>
    <select><option>No</option><option>Yes — described below</option></select>
    <label>Did you use AI assistance, and how?</label>
    <select><option>None</option><option>Drafting help, reviewed and rewritten by me</option>
      <option>Research only</option></select>
    <label>Is this your own original work, not published elsewhere?</label>
    <select><option>Yes</option><option>Published elsewhere — details below</option></select>
    <p class="btnrow" style="margin-top:16px"><button class="btn" data-demo="sub">Submit for review</button></p>
    <div id="subout"></div>
  </div>

  <div class="card2">
    <h2>Editor thread — “Recovering a slipped programme”</h2>
    <ul class="chat">
      <li><span class="who">R. Vance</span><p class="msg">Strong piece. Section three asserts a 12%
        recovery rate — can you attribute that, or shall we cut the number and keep the argument?</p></li>
      <li><span class="who">You</span><p class="msg">It's from our own portfolio, not published.
        I'll cut the figure and rewrite the paragraph around the mechanism.</p></li>
    </ul>
    <label for="msg">Reply</label>
    <textarea id="msg" placeholder="Write to your editor…"></textarea>
    <p class="btnrow" style="margin-top:10px"><button class="btn ghost">Send</button></p>
  </div>`,
  wire(root) {
    root.querySelector('[data-demo=sub]').addEventListener('click', () => {
      root.querySelector('#subout').innerHTML =
        `<div class="notice held" role="status">Submitted for editorial review. Every contributor
         article is read by an editor before publication; nothing you submit appears on the site until
         a person accepts it.</div>`;
    });
  }
},

editor: {
  kicker: 'Publishing', title: 'Editor\'s queue', path: 'proposed — /world-admin → Editorial',
  gates: ['A contributor can <b>never</b> approve or publish their own article',
          'The check spans two account systems — the hard part of the whole feature',
          'Every decision note is kept; a later note never overwrites an earlier one'],
  lede: `The editor's side, and the reason this phase existed. The refusal at the bottom is the one
         that matters: the reviewer here is also the author, under a different account, and the
         system knows because the link between the two was declared.`,
  html: `
  <div class="card2">
    <h2>Waiting for review</h2>
    <div class="scroll"><table>
      <thead><tr><th>Manuscript</th><th>Contributor</th><th>Waiting</th><th>State</th><th></th></tr></thead>
      <tbody>
        <tr><td>Recovering a slipped programme without lying about the date</td>
          <td>D. Okoro</td><td class="num">2 days</td>
          <td><span class="pill held">In review</span></td>
          <td><button class="btn ghost">Open</button></td></tr>
        <tr><td>Contingency drawdown in practice</td><td>P. Raman</td><td class="num">5 days</td>
          <td><span class="pill live">Approved</span></td>
          <td><button class="btn">Publish</button></td></tr>
        <tr style="background:var(--stop-bg)"><td>What a good estimate basis contains — part two</td>
          <td>R. Vance <span class="pill stop" style="margin-left:4px">You</span></td>
          <td class="num">1 day</td><td><span class="pill held">In review</span></td>
          <td><button class="btn" data-demo="mc">Approve</button></td></tr>
      </tbody>
    </table></div>
    <div id="mcout"></div>
  </div>

  <div class="card2">
    <h2>Contributor applications</h2>
    <ul class="rows">
      <li>
        <p style="margin:0 0 4px"><b>T. Nowak</b> <span class="pill held" style="margin-left:6px">Applied</span></p>
        <p class="muted" style="margin:0 0 8px">“Fifteen years in rail planning. I want to write about
          what actually happens to a programme after sanction.”</p>
        <p class="muted num" style="margin:0 0 10px">Forum standing: 34 accepted posts · 0 upheld reports</p>
        <p class="btnrow"><button class="btn">Grant standing</button>
          <button class="btn ghost">Decline</button></p>
        <p class="help">Forum standing is evidence for your decision. It is never the decision —
          an earned-only gate is something an abuser can play for.</p>
      </li>
    </ul>
  </div>`,
  wire(root) {
    root.querySelector('[data-demo=mc]').addEventListener('click', () => {
      root.querySelector('#mcout').innerHTML =
        `<div class="notice stop" role="status">You cannot approve your own submission. This manuscript
         was written by the PCI World account linked to your editor account, so it needs a second
         editor.</div>
         <p class="help">In the real system this refusal spans two separate account tables. Comparing
         the ids directly would never match — which is exactly how a control like this ends up looking
         correct while doing nothing.</p>`;
    });
  }
},

/* ══════════════════════════ ADMIN ══════════════════════════ */
admin: {
  kicker: 'Operator', title: 'World Admin', path: '/world-admin',
  gates: ['Every feature below ships <b>off</b>', 'Each can be switched without touching the database',
          'Two of them refuse to run until real policy text is published'],
  lede: `What an operator sees. The switches matter as much as the features: a flag that can only be
         moved by a database write is not a switch, and the direction that counts is off, during an
         incident.`,
  html: `
  <div class="card2">
    <h2>Feature switches</h2>
    <div class="scroll"><table>
      <thead><tr><th>Surface</th><th>State</th><th>Blocked on</th><th></th></tr></thead>
      <tbody>
        <tr><td>Community rooms</td><td><span class="pill stop">Off</span></td>
          <td class="muted">A contracted moderation provider; counsel's minimum age and country list</td>
          <td><button class="btn ghost">Turn on</button></td></tr>
        <tr><td>Community images</td><td><span class="pill stop">Off</span></td>
          <td class="muted">The same two decisions</td><td><button class="btn ghost">Turn on</button></td></tr>
        <tr><td>Forum</td><td><span class="pill stop">Off</span></td>
          <td class="muted">Nothing — ready when you are</td>
          <td><button class="btn">Turn on</button></td></tr>
        <tr><td>Careers</td><td><span class="pill stop">Off</span></td>
          <td class="muted">What "verified employer" means, in writing</td>
          <td><button class="btn ghost">Turn on</button></td></tr>
        <tr><td>Contributor publishing</td><td><span class="pill stop">Off</span></td>
          <td class="muted">Editorial policy and contributor terms</td>
          <td><button class="btn ghost">Turn on</button></td></tr>
      </tbody>
    </table></div>
  </div>

  <div class="card2">
    <h2>Policy text</h2>
    <p class="muted" style="margin:0 0 12px">Both are empty. Until each holds real published wording,
      the matching feature refuses rather than record someone's agreement to nothing.</p>
    <div class="scroll"><table>
      <thead><tr><th>Document</th><th>Version</th><th>Effect while empty</th></tr></thead>
      <tbody>
        <tr><td>Applicant privacy notice</td><td class="muted">— not set —</td>
          <td>Applying to a job answers “not open yet”</td></tr>
        <tr><td>Contributor terms</td><td class="muted">— not set —</td>
          <td>Applying to contribute answers “not open yet”</td></tr>
      </tbody>
    </table></div>
  </div>

  <div class="card2">
    <h2>Queues</h2>
    <div class="grid2">
      <div><p class="muted" style="margin:0">Forum posts held</p>
        <p class="num" style="font:400 32px/1.1 var(--serif);margin:4px 0 0">3</p></div>
      <div><p class="muted" style="margin:0">Employers awaiting verification</p>
        <p class="num" style="font:400 32px/1.1 var(--serif);margin:4px 0 0">1</p></div>
      <div><p class="muted" style="margin:0">Contributor applications</p>
        <p class="num" style="font:400 32px/1.1 var(--serif);margin:4px 0 0">1</p></div>
      <div><p class="muted" style="margin:0">Room reports</p>
        <p class="num" style="font:400 32px/1.1 var(--serif);margin:4px 0 0">0</p></div>
    </div>
  </div>`
},

};

const API = {"rooms": [["GET /api/world/community/rooms","built"]],"room": [["GET /api/world/community/rooms/{slug}","built"],["GET /api/world/community/rooms/{slug}/messages","built"],["POST /api/world/community/rooms/{slug}/messages","built"],["POST /api/world/community/media","built"]],"forum": [["GET /world/forum  (server-rendered page)","built"],["GET /api/world/forum/categories","built"]],"thread": [["GET /world/forum/{category}/{thread}  (server-rendered)","built"]],"composer": [["GET /api/world/forum/me","built"],["POST /api/world/forum/categories/{slug}/threads","built"],["POST /api/world/forum/threads/{id}/posts","built"]],"jobs": [["GET /world/careers  (server-rendered page)","built"],["GET /world-sitemap.xml  (live jobs only)","built"]],"job": [["GET /world/careers/{employer}/{slug}  (server-rendered)","built"],["POST /api/world/careers/postings/{id}/apply","built"]],"employer": [["GET/POST /api/world/careers/employers","built"],["GET/POST /api/world/careers/employers/{id}/postings","built"],["POST /api/world/careers/postings/{id}/publish","built"],["GET /api/world/careers/postings/{id}/applications","built"],["GET /api/world/careers/applications/{id}/cv","built"],["POST /api/world/careers/applications/{id}/decision","built"]],"apps": [["GET /api/world/careers/my/applications","built"],["POST /api/world/careers/applications/{id}/withdraw","built"],["POST /api/world/careers/applications/{id}/consent/withdraw","built"]],"contrib": [["GET /api/world/contributor/me","built"],["POST /api/world/contributor/apply","built"],["GET/POST /api/world/contributor/articles","built"],["PATCH /api/world/contributor/articles/{id}","built"],["POST /api/world/contributor/articles/{id}/submit","built"],["POST /api/world/contributor/articles/{id}/messages","built"]],"editor": [["GET /api/world-admin/editorial/contributors","built"],["POST /api/world-admin/editorial/contributors/{id}/decision","built"],["POST /api/world-admin/articles/{id}/approve","built"],["POST /api/world-admin/articles/{id}/publish","built"],["this SCREEN","not built \u2014 API only today"]],"admin": [["GET/PATCH /api/world-admin/community/settings","built"],["POST /api/world-admin/careers/employers/{id}/state","built"],["GET /api/world-admin/forum/queue","built"]]};

function apiTable(key) {
  const rows = API[key] || [];
  if (!rows.length) return '';
  // The single most useful thing on the page for whoever picks up the server side: which calls this
  // screen needs, and whether they already exist. Almost all of them do.
  return `<section class="apis"><h2>What this screen calls</h2>
    <div class="scroll"><table><thead><tr><th>Endpoint</th><th>Status</th></tr></thead><tbody>
    ${rows.map(([e, st]) => `<tr><td><code>${e}</code></td><td>${
      st === 'built' ? '<span class="pill live">Built</span>'
                     : `<span class="pill stop">${st}</span>`}</td></tr>`).join('')}
    </tbody></table></div></section>`;
}

const stage = document.getElementById('stage');
const rail = document.querySelector('.rail');

function render(key) {
  const s = S[key]; if (!s) return;
  stage.innerHTML = `
    <header class="shead">
      <p class="kicker">${s.kicker}</p>
      <h1>${s.title}</h1>
      <p class="path">${s.path}</p>
    </header>
    <div class="gate">${s.gates.map(g => `<span class="g">${g}</span>`).join('')}</div>
    <div class="body">
      <p class="lede">${s.lede}</p>
      ${s.html}
      ${apiTable(key)}
      <div class="foot"><b>This page is a mock-up.</b> Nothing here is stored, sent or checked — the
        buttons show what the real screens say, not what a server decided. The rules in the amber
        strip above are enforced by the actual system; this drawing of them cannot enforce anything.</div>
    </div>`;
  if (s.wire) s.wire(stage);
  rail.querySelectorAll('button[data-s]').forEach(b =>
    b.setAttribute('aria-current', b.dataset.s === key ? 'true' : 'false'));
  stage.scrollIntoView({ block: 'start' });
}

// Hash routing rather than paths: this is a static file, so there is no server to teach about
// /world/preview/jobs — and a hash still gives every screen a URL somebody can send to a developer.
function go(key) { if (S[key] && location.hash.slice(2) !== key) location.hash = '#/' + key; else render(key); }

rail.addEventListener('click', e => {
  const b = e.target.closest('button[data-s]'); if (b) go(b.dataset.s);
});
stage.addEventListener('click', e => {
  const g = e.target.closest('[data-go]');
  if (g) { e.preventDefault(); go(g.dataset.go); }
});
window.addEventListener('hashchange', () => render(location.hash.slice(2) || 'rooms'));

render(location.hash.slice(2) || 'rooms');
