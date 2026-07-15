/* ===== PCI-CHAT: self-hosted website chat widget =====
   A floating crimson chat bubble (bottom-right) on every public page. Clicking opens a panel
   that talks to the first-party chat API (Endpoints/Chat.cs) — no third-party services:
     POST /api/chat/start  {name?}            -> {token, greeting, status}
     POST /api/chat/send   {token, body, escalate?} -> {status, replies:[{id,sender,body}]}
     GET  /api/chat/poll?token=&after=        -> {status, visitor_name, messages:[{id,sender,body,created_at}]}
   A bot answers first from the admin-managed knowledge base; the visitor can ask for a person
   ("Talk to a person" -> escalate) and a team member replies from the admin console. The token
   is kept in localStorage (pci_chat_token) so the conversation survives navigation; reopening
   restores the full history via poll after=0. Polls every 4s WHILE the panel is open.

   Rendering rule: ALL server/user/bot content is inserted with textContent / createElement only.
   innerHTML is used solely for a couple of static, developer-authored inline SVG icons — never
   with any message, status or name that comes from the network or the user. */
(function () {
  'use strict';
  try {
    if (window.__pciChatLoaded) return;
    window.__pciChatLoaded = true;

    var API = (window.PCI_API_BASE || '').replace(/\/$/, '');
    var TOKEN_KEY = 'pci_chat_token';
    var POLL_MS = 4000;
    var PRIVACY = "Conversations are stored so our team can follow up. Please don't share confidential information.";
    var STATUS_LABEL = {
      bot: 'PCI Assistant',
      waiting: 'Waiting for the team…',
      live: 'Live chat',
      closed: 'Chat closed'
    };

    var token = null;
    try { token = localStorage.getItem(TOKEN_KEY) || null; } catch (e) { token = null; }

    var state = {
      open: false,
      started: false,     // start POSTed (or restored) this session
      lastId: 0,          // highest message id rendered
      status: 'bot',
      sending: false,
      pollTimer: null,
      inflight: false
    };

    // ---- element refs (populated by build) ----
    var els = {};

    // ---------- small helpers ----------
    function css(name, fallback) {
      try {
        var v = getComputedStyle(document.documentElement).getPropertyValue(name);
        v = (v || '').trim();
        return v || fallback;
      } catch (e) { return fallback; }
    }
    function el(tag, cls, text) {
      var n = document.createElement(tag);
      if (cls) n.className = cls;
      if (text != null) n.textContent = text;
      return n;
    }
    function setToken(t) {
      token = t;
      try { if (t) localStorage.setItem(TOKEN_KEY, t); else localStorage.removeItem(TOKEN_KEY); } catch (e) { /* private mode */ }
    }
    function fmtTime(s) {
      if (!s) return '';
      var d = new Date(String(s).replace(' ', 'T') + (String(s).indexOf('Z') < 0 && String(s).indexOf('T') > 0 ? 'Z' : ''));
      if (isNaN(d.getTime())) { d = new Date(String(s).replace(' ', 'T')); }
      if (isNaN(d.getTime())) return '';
      try {
        return d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
      } catch (e) { return ''; }
    }

    // ---------- network ----------
    function apiJson(method, path, body) {
      var opts = { method: method, headers: {} };
      if (body !== undefined) {
        opts.headers['Content-Type'] = 'application/json';
        opts.body = JSON.stringify(body);
      }
      return fetch(API + path, opts).then(function (r) {
        return r.json().catch(function () { return {}; }).then(function (j) {
          return { status: r.status, ok: r.ok, body: j };
        });
      });
    }

    // ---------- styles ----------
    function injectStyles() {
      if (document.getElementById('pci-chat-style')) return;
      var crimson = css('--crimson', '#C13329');
      var ink = css('--ink', '#0F172A');
      var blue = css('--blue', '') || css('--red', '') || '#1D4ED8';
      var sans = css('--sans', "'Inter',system-ui,-apple-system,'Segoe UI',Roboto,sans-serif");
      var st = document.createElement('style');
      st.id = 'pci-chat-style';
      st.textContent = [
        '.pci-chat-bubble{position:fixed;right:20px;bottom:20px;z-index:2147483000;width:58px;height:58px;border-radius:50%;',
        'border:none;cursor:pointer;background:' + crimson + ';color:#fff;box-shadow:0 8px 26px rgba(15,23,42,.28);',
        'display:flex;align-items:center;justify-content:center;transition:transform .16s ease,box-shadow .16s ease}',
        '.pci-chat-bubble:hover{transform:translateY(-2px);box-shadow:0 12px 32px rgba(15,23,42,.34)}',
        '.pci-chat-bubble:focus-visible{outline:3px solid rgba(29,78,216,.5);outline-offset:2px}',
        '.pci-chat-bubble svg{width:26px;height:26px}',
        '.pci-chat-bubble .pci-chat-dot{position:absolute;top:6px;right:6px;width:12px;height:12px;border-radius:50%;',
        'background:' + blue + ';border:2px solid #fff;display:none}',
        '.pci-chat-bubble.has-unread .pci-chat-dot{display:block}',

        '.pci-chat-panel{position:fixed;right:20px;bottom:88px;z-index:2147483000;width:360px;max-width:calc(100vw - 32px);',
        'height:520px;max-height:calc(100vh - 120px);background:#fff;border-radius:16px;overflow:hidden;',
        'box-shadow:0 24px 70px rgba(15,23,42,.28);border:1px solid #E3E8EF;display:none;flex-direction:column;',
        'font-family:' + sans + ';color:' + ink + ';font-size:14px;line-height:1.5}',
        '.pci-chat-panel.open{display:flex}',

        '.pci-chat-head{background:' + blue + ';color:#fff;padding:14px 16px;display:flex;align-items:center;gap:11px;flex:0 0 auto}',
        '.pci-chat-head .pci-chat-avatar{width:34px;height:34px;border-radius:50%;background:rgba(255,255,255,.18);',
        'display:flex;align-items:center;justify-content:center;flex:0 0 auto}',
        '.pci-chat-head .pci-chat-avatar svg{width:19px;height:19px}',
        '.pci-chat-head .pci-chat-htext{min-width:0;flex:1}',
        '.pci-chat-head .pci-chat-title{font-weight:700;font-size:14.5px;letter-spacing:.2px}',
        '.pci-chat-head .pci-chat-status{font-size:11.5px;opacity:.9;display:flex;align-items:center;gap:6px;margin-top:2px}',
        '.pci-chat-head .pci-chat-status .pci-chat-sdot{width:7px;height:7px;border-radius:50%;background:#fff;opacity:.85}',
        '.pci-chat-close{background:none;border:none;color:#fff;cursor:pointer;opacity:.85;padding:4px;border-radius:6px;flex:0 0 auto}',
        '.pci-chat-close:hover{opacity:1;background:rgba(255,255,255,.14)}',
        '.pci-chat-close svg{width:18px;height:18px;display:block}',

        '.pci-chat-log{flex:1 1 auto;overflow-y:auto;padding:14px;background:#f6f8fc;display:flex;flex-direction:column;gap:9px}',
        '.pci-chat-msg{max-width:82%;padding:9px 12px;border-radius:14px;white-space:pre-wrap;word-wrap:break-word;overflow-wrap:anywhere}',
        '.pci-chat-msg .pci-chat-meta{display:block;font-size:10px;opacity:.6;margin-top:4px}',
        '.pci-chat-msg.visitor{align-self:flex-end;background:' + blue + ';color:#fff;border-bottom-right-radius:4px}',
        '.pci-chat-msg.visitor .pci-chat-meta{color:#fff}',
        '.pci-chat-msg.bot{align-self:flex-start;background:#fff;border:1px solid #E3E8EF;color:' + ink + ';border-bottom-left-radius:4px}',
        '.pci-chat-msg.agent{align-self:flex-start;background:#fff;border:1px solid ' + crimson + ';color:' + ink + ';border-bottom-left-radius:4px}',
        '.pci-chat-msg.agent .pci-chat-who{display:block;font-size:10.5px;font-weight:700;color:' + crimson + ';margin-bottom:2px}',
        '.pci-chat-note{align-self:center;font-size:12px;color:#64748B;background:#eef2f8;border-radius:10px;padding:6px 11px;max-width:92%;text-align:center}',

        '.pci-chat-foot{flex:0 0 auto;border-top:1px solid #E3E8EF;background:#fff}',
        '.pci-chat-actions{padding:8px 10px 0}',
        '.pci-chat-human{width:100%;border:1px solid ' + crimson + ';background:#fff;color:' + crimson + ';font-weight:600;',
        'font-family:inherit;font-size:12.5px;border-radius:9px;padding:8px 10px;cursor:pointer;display:flex;align-items:center;justify-content:center;gap:7px}',
        '.pci-chat-human:hover{background:' + crimson + ';color:#fff}',
        '.pci-chat-human:disabled{opacity:.5;cursor:default}',
        '.pci-chat-human svg{width:15px;height:15px}',
        '.pci-chat-inrow{display:flex;gap:8px;align-items:flex-end;padding:9px 10px}',
        '.pci-chat-input{flex:1;resize:none;border:1.5px solid #E3E8EF;border-radius:10px;padding:9px 11px;',
        'font-family:inherit;font-size:13.5px;line-height:1.4;max-height:96px;color:' + ink + '}',
        '.pci-chat-input:focus{outline:none;border-color:' + blue + ';box-shadow:0 0 0 3px rgba(29,78,216,.14)}',
        '.pci-chat-send{flex:0 0 auto;border:none;background:' + crimson + ';color:#fff;border-radius:10px;width:40px;height:40px;',
        'cursor:pointer;display:flex;align-items:center;justify-content:center}',
        '.pci-chat-send:hover{filter:brightness(1.06)}',
        '.pci-chat-send:disabled{opacity:.5;cursor:default}',
        '.pci-chat-send svg{width:18px;height:18px}',
        '.pci-chat-privacy{padding:0 12px 10px;font-size:10.5px;color:#8892a4;text-align:center;line-height:1.35}',

        '@media (max-width:480px){.pci-chat-panel{right:8px;left:8px;width:auto;bottom:82px;height:calc(100vh - 100px)}}'
      ].join('');
      document.head.appendChild(st);
    }

    // ---------- inline SVG icons (static, developer-authored) ----------
    function chatIconSvg() {
      return '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">' +
        '<path d="M21 11.5a8.38 8.38 0 0 1-8.5 8.5 9 9 0 0 1-4-1L3 20l1.5-5.5a8.38 8.38 0 0 1-1-4A8.5 8.5 0 0 1 12 2.5a8.38 8.38 0 0 1 8.5 8.5z"/></svg>';
    }
    function personIconSvg() {
      return '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">' +
        '<path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>';
    }
    function sendIconSvg() {
      return '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">' +
        '<path d="M22 2 11 13M22 2l-7 20-4-9-9-4 20-7z"/></svg>';
    }
    function closeIconSvg() {
      return '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true">' +
        '<path d="M18 6 6 18M6 6l12 12"/></svg>';
    }

    // ---------- build DOM ----------
    function build() {
      injectStyles();

      var bubble = document.createElement('button');
      bubble.type = 'button';
      bubble.className = 'pci-chat-bubble';
      bubble.setAttribute('aria-label', 'Open chat with the PCI Assistant');
      bubble.setAttribute('aria-haspopup', 'dialog');
      bubble.setAttribute('aria-expanded', 'false');
      bubble.innerHTML = chatIconSvg() + '<span class="pci-chat-dot" aria-hidden="true"></span>';
      bubble.addEventListener('click', function () { toggle(); });
      els.bubble = bubble;
      els.bubbleDot = bubble.querySelector('.pci-chat-dot');

      var panel = document.createElement('div');
      panel.className = 'pci-chat-panel';
      panel.setAttribute('role', 'dialog');
      panel.setAttribute('aria-label', 'PCI Assistant chat');
      panel.hidden = false;

      // header
      var head = el('div', 'pci-chat-head');
      var avatar = el('div', 'pci-chat-avatar');
      avatar.innerHTML = chatIconSvg();
      var htext = el('div', 'pci-chat-htext');
      htext.appendChild(el('div', 'pci-chat-title', 'PCI Assistant'));
      var statusLine = el('div', 'pci-chat-status');
      statusLine.appendChild(el('span', 'pci-chat-sdot'));
      var statusText = el('span', null, STATUS_LABEL.bot);
      statusLine.appendChild(statusText);
      htext.appendChild(statusLine);
      var closeBtn = el('button', 'pci-chat-close');
      closeBtn.type = 'button';
      closeBtn.setAttribute('aria-label', 'Close chat');
      closeBtn.innerHTML = closeIconSvg();
      closeBtn.addEventListener('click', function () { close(); });
      head.appendChild(avatar);
      head.appendChild(htext);
      head.appendChild(closeBtn);

      // message log
      var log = el('div', 'pci-chat-log');
      log.setAttribute('role', 'log');
      log.setAttribute('aria-live', 'polite');
      log.setAttribute('aria-label', 'Chat messages');

      // footer
      var foot = el('div', 'pci-chat-foot');
      var actions = el('div', 'pci-chat-actions');
      var humanBtn = el('button', 'pci-chat-human');
      humanBtn.type = 'button';
      humanBtn.innerHTML = personIconSvg() + '<span>Talk to a person</span>';
      humanBtn.addEventListener('click', function () { escalate(); });
      actions.appendChild(humanBtn);

      var inrow = el('div', 'pci-chat-inrow');
      var input = document.createElement('textarea');
      input.className = 'pci-chat-input';
      input.rows = 1;
      input.setAttribute('aria-label', 'Type your message');
      input.setAttribute('placeholder', 'Type your message…');
      input.addEventListener('input', autosize);
      input.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !e.shiftKey) { e.preventDefault(); send(); }
      });
      var sendBtn = el('button', 'pci-chat-send');
      sendBtn.type = 'button';
      sendBtn.setAttribute('aria-label', 'Send message');
      sendBtn.innerHTML = sendIconSvg();
      sendBtn.addEventListener('click', function () { send(); });
      inrow.appendChild(input);
      inrow.appendChild(sendBtn);

      var privacy = el('div', 'pci-chat-privacy', PRIVACY);

      foot.appendChild(actions);
      foot.appendChild(inrow);
      foot.appendChild(privacy);

      panel.appendChild(head);
      panel.appendChild(log);
      panel.appendChild(foot);

      document.body.appendChild(bubble);
      document.body.appendChild(panel);

      els.panel = panel;
      els.statusText = statusText;
      els.log = log;
      els.input = input;
      els.sendBtn = sendBtn;
      els.humanBtn = humanBtn;
    }

    function autosize() {
      var t = els.input;
      t.style.height = 'auto';
      t.style.height = Math.min(96, t.scrollHeight) + 'px';
    }

    // ---------- rendering (textContent / createElement ONLY for dynamic content) ----------
    function nearBottom() {
      var l = els.log;
      return (l.scrollHeight - l.scrollTop - l.clientHeight) < 60;
    }
    function scrollToBottom() {
      els.log.scrollTop = els.log.scrollHeight;
    }
    function renderMessage(m) {
      var sender = m.sender === 'visitor' ? 'visitor' : (m.sender === 'agent' ? 'agent' : 'bot');
      var wrap = el('div', 'pci-chat-msg ' + sender);
      if (sender === 'agent') {
        wrap.appendChild(el('span', 'pci-chat-who', 'Team'));
      }
      var bodyNode = document.createElement('span');
      bodyNode.textContent = m.body == null ? '' : String(m.body); // textContent — never innerHTML
      wrap.appendChild(bodyNode);
      var t = fmtTime(m.created_at);
      if (t) wrap.appendChild(el('span', 'pci-chat-meta', t));
      els.log.appendChild(wrap);
    }
    function renderNote(text) {
      els.log.appendChild(el('div', 'pci-chat-note', text));
    }
    function setStatus(status) {
      state.status = status;
      var label = STATUS_LABEL[status] || STATUS_LABEL.bot;
      els.statusText.textContent = label;
      var closed = status === 'closed';
      els.input.disabled = closed;
      els.sendBtn.disabled = closed || state.sending;
      els.humanBtn.disabled = closed || status === 'waiting' || status === 'live';
      if (closed) els.input.setAttribute('placeholder', 'This chat has been closed.');
      else els.input.setAttribute('placeholder', 'Type your message…');
    }

    // ---------- flows ----------
    function open() {
      state.open = true;
      els.panel.classList.add('open');
      els.bubble.setAttribute('aria-expanded', 'true');
      els.bubble.classList.remove('has-unread');
      ensureStarted().then(function () {
        startPolling();
        try { els.input.focus(); } catch (e) { /* ignore */ }
      });
    }
    function close() {
      state.open = false;
      els.panel.classList.remove('open');
      els.bubble.setAttribute('aria-expanded', 'false');
      stopPolling();
    }
    function toggle() { if (state.open) close(); else open(); }

    // Start the conversation (or restore an existing one). Returns a promise.
    function ensureStarted() {
      if (state.started) return Promise.resolve();
      if (token) {
        // Restore existing conversation: full history via after=0.
        state.started = true;
        return pollOnce(true).then(function (found) {
          if (!found) {
            // token no longer valid (e.g. purged): start fresh
            state.started = false;
            setToken(null);
            return ensureStarted();
          }
        });
      }
      // Fresh start
      return apiJson('POST', '/api/chat/start', {}).then(function (res) {
        if (res.status === 429) { renderNote("We're a little busy right now — please try again in a moment."); return; }
        if (!res.ok || !res.body || !res.body.token) { renderNote("Sorry — the chat couldn't be started. Please try again shortly."); return; }
        setToken(res.body.token);
        state.started = true;
        setStatus(res.body.status || 'bot');
        if (res.body.greeting) {
          renderMessage({ sender: 'bot', body: res.body.greeting });
          scrollToBottom();
        }
      }).catch(function () {
        renderNote("Sorry — we couldn't reach the chat service. Please check your connection and try again.");
      });
    }

    function send() {
      var text = els.input.value.trim();
      if (!text || state.sending || !token || state.status === 'closed') return;
      state.sending = true;
      els.sendBtn.disabled = true;
      var payload = { token: token, body: text };
      els.input.value = '';
      autosize();
      apiJson('POST', '/api/chat/send', payload).then(function (res) {
        state.sending = false;
        els.sendBtn.disabled = state.status === 'closed';
        if (res.status === 429) { renderNote("You've sent a lot of messages — please pause for a moment before sending more."); scrollToBottom(); return; }
        if (res.status === 409) { setStatus('closed'); renderNote('This chat has been closed by the team.'); return; }
        if (!res.ok) { renderNote("That message couldn't be sent. Please try again."); return; }
        // Fetch the stored visitor message + any bot/agent replies in order.
        pollOnce();
      }).catch(function () {
        state.sending = false;
        els.sendBtn.disabled = state.status === 'closed';
        renderNote("That message couldn't be sent — please check your connection.");
      });
    }

    function escalate() {
      if (!token || state.status === 'closed') return;
      if (state.status === 'waiting' || state.status === 'live') return;
      els.humanBtn.disabled = true;
      apiJson('POST', '/api/chat/send', { token: token, body: 'I would like to talk to a person, please.', escalate: true })
        .then(function (res) {
          if (res.status === 429) { renderNote("We're a little busy — please try again in a moment."); els.humanBtn.disabled = false; return; }
          if (!res.ok) { renderNote("Couldn't pass you to the team just now — please try again."); els.humanBtn.disabled = false; return; }
          pollOnce();
        }).catch(function () {
          renderNote("Couldn't reach the team just now — please check your connection.");
          els.humanBtn.disabled = false;
        });
    }

    // Fetch new messages after state.lastId (or full history when restore=true, after=0).
    function pollOnce(restore) {
      if (!token || state.inflight) return Promise.resolve(true);
      state.inflight = true;
      var after = restore ? 0 : state.lastId;
      return apiJson('GET', '/api/chat/poll?token=' + encodeURIComponent(token) + '&after=' + after)
        .then(function (res) {
          state.inflight = false;
          if (res.status === 404) { return false; }
          if (!res.ok || !res.body) { return true; }
          var msgs = res.body.messages || [];
          var stick = nearBottom();
          var appended = false;
          for (var i = 0; i < msgs.length; i++) {
            var m = msgs[i];
            if (m.id > state.lastId) {
              renderMessage(m);
              state.lastId = m.id;
              appended = true;
            }
          }
          if (res.body.status) setStatus(res.body.status);
          if (appended && (stick || restore)) scrollToBottom();
          if (appended && !state.open) els.bubble.classList.add('has-unread');
          return true;
        }).catch(function () {
          state.inflight = false;
          return true;
        });
    }

    function startPolling() {
      stopPolling();
      state.pollTimer = setInterval(function () {
        if (!state.open) { stopPolling(); return; }
        if (state.status === 'closed') return; // nothing new will arrive
        pollOnce();
      }, POLL_MS);
    }
    function stopPolling() {
      if (state.pollTimer) { clearInterval(state.pollTimer); state.pollTimer = null; }
    }

    // ---------- boot ----------
    function boot() { build(); }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
  } catch (e) { /* no-op: page stays fully usable without chat */ }
})();
/* ===== /PCI-CHAT ===== */
