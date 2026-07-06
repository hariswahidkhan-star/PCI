#!/usr/bin/env python3
"""Split the single-file PCI SPA into real, SEO-ready static pages."""
import os, re, json, html as H, datetime, shutil
from bs4 import BeautifulSoup, Comment

SRC   = '/mnt/user-data/uploads/PCI_Website_Complete__1_.html'
OUT   = '/home/claude/site'
BASE  = 'https://www.projectcontrolsinstitute.org/'
TODAY = datetime.date.today().isoformat()

raw  = open(SRC, encoding='utf-8').read()

# replace leftover prototype captions on the thin utility pages with honest, useful copy
RAW_REPLACEMENTS = [
 ("Complete the fields below. In this design prototype, forms are routed via the contact page; a live site would offer secure online submission and a downloadable PDF version.",
  "Send your submission using the contact form, or email hello@projectcontrolsinstitute.org, including the details listed above. You\u2019ll receive an email acknowledgement with a reference number you can use to track progress."),
 ("In line with our Data Protection Policy; a live site would confirm secure handling.",
  "We handle your information under our Privacy Policy and Data Protection Policy: it is used only to process your submission, shared only with those who need it to respond, and kept no longer than necessary."),
 ("A live site would provide a downloadable PDF; here the form is presented online.",
  "You can submit online using the details above. If you need a paper copy or another format, contact us and we\u2019ll arrange it."),
 ("Illustrative result. In this design prototype, lookups are not connected to a live registry.",
  "Illustrative result shown for layout. Live verification becomes available as PCP-AI credentials are issued; each lookup then checks the credential against the PCI registry."),
 ("Placeholder details for the design prototype.",
  "Prefer email? Write to hello@projectcontrolsinstitute.org and we\u2019ll route your message to the right team."),
 ("We aim to respond promptly; in this prototype, contact details are placeholders.",
  "We aim to acknowledge enquiries within two working days. Prefer email? Write to hello@projectcontrolsinstitute.org."),
]
for _old, _new in RAW_REPLACEMENTS:
    assert _old in raw, 'replacement string not found: ' + _old[:48]
    raw = raw.replace(_old, _new)

soup = BeautifulSoup(raw, 'lxml')
pmeta = json.load(open('/home/claude/pmeta.json'))

# ---------------------------------------------------------------- routes
sections = soup.select('section.page')
routes   = [s['id'][2:] for s in sections]           # strip 'p-'
routeset = set(routes)
KEYWORDS = (soup.find('meta', attrs={'name': 'keywords'}) or {}).get('content', '')

def route_path(r): return '/' if r == 'home' else '/' + r

# ================= added website features (cookie / search / newsletter / downloads) =================
EXTRA_CSS = '''
/* ---- newsletter band ---- */
.nl-band{background:#0f1319;color:#fff;padding:46px 0}
.nl-in{max-width:1180px;margin:0 auto;padding:0 24px;display:flex;align-items:center;justify-content:space-between;gap:28px;flex-wrap:wrap}
.nl-in h3{font-size:24px;margin:0 0 6px;font-weight:800;letter-spacing:-.02em}
.nl-in p{margin:0;color:#aeb5bf;font-size:14px;max-width:48ch;line-height:1.5}
.nl-form{display:flex;gap:10px;flex-wrap:wrap}
.nl-form input{height:48px;border-radius:10px;border:1px solid #2a3340;background:#161b22;color:#fff;padding:0 14px;font-size:14px;min-width:260px}
.nl-form input:focus{outline:0;border-color:#D8261C}
.nl-form button{height:48px;border-radius:10px;border:0;background:#D8261C;color:#fff;font-weight:700;padding:0 22px;font-size:14px;cursor:pointer}
.nl-form button:hover{background:#B41E16}
.nl-msg{font-size:13px;font-weight:600;margin-top:10px;width:100%;min-height:18px}
/* ---- cookie banner ---- */
.ck-banner{position:fixed;left:16px;right:16px;bottom:16px;max-width:540px;margin:0 auto;background:#fff;border:1px solid #e7e9ee;border-radius:14px;box-shadow:0 18px 50px rgba(16,22,30,.22);padding:18px 20px;z-index:9000;display:none}
.ck-banner.show{display:block;animation:ckin .3s ease}
@keyframes ckin{from{opacity:0;transform:translateY(10px)}to{opacity:1;transform:none}}
.ck-banner h4{margin:0 0 6px;font-size:15px;font-weight:800;color:#11151C}
.ck-banner p{margin:0 0 14px;font-size:13px;color:#5c6672;line-height:1.55}
.ck-banner a{color:#D8261C}
.ck-row{display:flex;gap:10px;flex-wrap:wrap}
.ck-row button{height:40px;border-radius:9px;font-weight:650;font-size:13px;padding:0 16px;cursor:pointer;border:1px solid #e7e9ee;background:#fff;color:#11151C}
.ck-row .ck-accept{background:#D8261C;border-color:#D8261C;color:#fff}.ck-row .ck-accept:hover{background:#B41E16}
.ck-row button:hover{border-color:#d3d8df}
/* ---- search modal ---- */
.sx-scrim{position:fixed;inset:0;background:rgba(13,17,23,.5);z-index:9500;display:none;align-items:flex-start;justify-content:center;padding:78px 16px}
.sx-scrim.show{display:flex}
.sx-box{width:100%;max-width:640px;background:#fff;border-radius:16px;box-shadow:0 30px 80px rgba(0,0,0,.35);overflow:hidden}
.sx-top{display:flex;align-items:center;gap:12px;padding:15px 18px;border-bottom:1px solid #eef0f4}
.sx-top input{flex:1;border:0;outline:0;font-size:17px;color:#11151C;background:transparent}
.sx-close{border:0;background:#f2f3f5;border-radius:8px;width:30px;height:30px;cursor:pointer;color:#5c6672;font-size:15px}
.sx-results{max-height:52vh;overflow-y:auto;padding:8px}
.sx-results a{display:block;padding:11px 12px;border-radius:9px;text-decoration:none;color:#11151C}
.sx-results a:hover{background:#f6f7f9}
.sx-results .st{font-weight:650;font-size:14px}
.sx-results .su{font-size:12px;color:#8a929e;font-family:ui-monospace,Menlo,monospace}
.sx-results .sd{font-size:12.5px;color:#5c6672;margin-top:2px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
.sx-empty{padding:24px;text-align:center;color:#8a929e;font-size:13px}
.sx-hint{padding:9px 16px 14px;font-size:11.5px;color:#aab1bb}
/* ---- downloads ---- */
.dl-grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(258px,1fr));gap:14px}
.dl-card{border:1px solid #e7e9ee;border-radius:12px;padding:16px;display:flex;gap:13px;align-items:flex-start;background:#fff;transition:.15s}
.dl-card:hover{border-color:#d3d8df;box-shadow:0 6px 18px rgba(16,22,30,.07)}
.dl-ic{width:42px;height:42px;border-radius:10px;background:#FCEBE9;color:#B41E16;display:flex;align-items:center;justify-content:center;flex:none;font-weight:800;font-size:11px;font-family:ui-monospace,monospace}
.dl-meta{flex:1;min-width:0}.dl-meta .dt{font-weight:650;font-size:14px;color:#11151C;line-height:1.3}.dl-meta .dd{font-size:12px;color:#8a929e;margin-top:3px}
.dl-card a{display:inline-flex;align-items:center;gap:6px;margin-top:9px;font-size:12.5px;font-weight:700;color:#D8261C;text-decoration:none}
'''

NEWSLETTER = ('<section class="nl-band"><div class="nl-in">'
  '<div><h3>Stay in the loop</h3><p>Occasional updates on the standard, exam windows and new chapters. No noise, unsubscribe anytime.</p></div>'
  '<div><div class="nl-form"><input type="email" id="nlEmail" placeholder="you@organisation.com" aria-label="Email address"/>'
  '<button type="button" id="nlBtn">Subscribe</button></div><div class="nl-msg" id="nlMsg"></div></div>'
  '</div></section>')

COOKIE = ('<div class="ck-banner" id="ckBanner" role="dialog" aria-label="Cookie notice">'
  '<h4>Cookies</h4><p>We use essential cookies to run this site, and optional analytics to improve it. '
  'See our <a href="/privacy">Privacy &amp; Cookie Policy</a>.</p>'
  '<div class="ck-row"><button class="ck-accept" id="ckAccept">Accept all</button>'
  '<button id="ckReject">Essential only</button></div></div>')

SEARCH_MODAL = ('<div class="sx-scrim" id="sxScrim" role="dialog" aria-label="Search">'
  '<div class="sx-box"><div class="sx-top">'
  '<svg width="20" height="20" fill="none" stroke="#8a929e" stroke-width="1.9" viewBox="0 0 24 24"><circle cx="11" cy="11" r="7"></circle><path d="M21 21l-4-4"></path></svg>'
  '<input type="search" id="sxInput" placeholder="Search certifications, chapters, resources…" aria-label="Search the site"/>'
  '<button class="sx-close" id="sxClose" aria-label="Close search">&#10005;</button></div>'
  '<div class="sx-results" id="sxResults"></div><div class="sx-hint">Press Esc to close · &#8984;K to open</div></div></div>')

FEATURE_JS = '''(function(){
 var nb=document.getElementById('nlBtn');
 if(nb){nb.addEventListener('click',function(){
   var e=document.getElementById('nlEmail'),m=document.getElementById('nlMsg');
   if(!e.value||e.value.indexOf('@')<1){m.style.color='#ffb4ad';m.textContent='Please enter a valid email address.';return;}
   m.style.color='#5fd0a0';m.textContent='Thanks — please check your inbox to confirm.';e.value='';
 });}
 try{
   if(!localStorage.getItem('pci-cookie-consent')){
     var b=document.getElementById('ckBanner');
     if(b){b.classList.add('show');var set=function(v){try{localStorage.setItem('pci-cookie-consent',v);}catch(e){}b.classList.remove('show');};
       document.getElementById('ckAccept').addEventListener('click',function(){set('all');});
       document.getElementById('ckReject').addEventListener('click',function(){set('essential');});}
   }
 }catch(e){}
 var scrim=document.getElementById('sxScrim'),inp=document.getElementById('sxInput'),res=document.getElementById('sxResults'),idx=null;
 function run(){
   if(!idx)return; var q=(inp.value||'').trim().toLowerCase();
   if(!q){res.innerHTML='<div class="sx-empty">Start typing to search '+idx.length+' pages.</div>';return;}
   var out=idx.filter(function(p){return (p.t+' '+p.d+' '+p.u).toLowerCase().indexOf(q)>=0;}).slice(0,8);
   if(!out.length){res.innerHTML='<div class="sx-empty">No pages match that search.</div>';return;}
   res.innerHTML=out.map(function(p){return '<a href="'+p.u+'"><div class="st">'+p.t+'</div><div class="su">'+p.u+'</div><div class="sd">'+(p.d||'')+'</div></a>';}).join('');
 }
 function openSx(){if(!scrim)return;scrim.classList.add('show');setTimeout(function(){inp.focus();},30);
   if(!idx){fetch('/search-index.json').then(function(r){return r.json();}).then(function(d){idx=d;run();}).catch(function(){idx=[];res.innerHTML='<div class="sx-empty">Search is unavailable.</div>';});}else{run();}}
 function closeSx(){if(scrim)scrim.classList.remove('show');}
 var sb=document.getElementById('siteSearchBtn'); if(sb)sb.addEventListener('click',openSx);
 if(inp)inp.addEventListener('input',run);
 var cl=document.getElementById('sxClose'); if(cl)cl.addEventListener('click',closeSx);
 if(scrim)scrim.addEventListener('click',function(e){if(e.target===scrim)closeSx();});
 document.addEventListener('keydown',function(e){
   if(e.key==='Escape')closeSx();
   if((e.key==='k'||e.key==='K')&&(e.metaKey||e.ctrlKey)){e.preventDefault();openSx();}
 });
})();'''

def dl_section(title, files):
    cards = ''.join('<div class="dl-card"><div class="dl-ic">%s</div><div class="dl-meta"><div class="dt">%s</div><div class="dd">%s</div><a href="%s">Download &#8595;</a></div></div>' % (ext, nm, meta, href) for (nm, ext, meta, href) in files)
    return ('<section class="sec sec-2"><div class="wrap"><span class="eyebrow">Downloads</span>'
            '<h2 style="margin:0 0 8px;font-size:clamp(26px,3vw,38px);">%s</h2>'
            '<div class="uline" style="margin-bottom:22px;"></div><div class="dl-grid">%s</div></div></section>') % (title, cards)

DOWNLOADS = {
 'white-papers': ('White papers', [
    ('Governed AI in Project Controls — 2026 Position Paper', 'PDF', 'PDF · 2.4 MB', '/downloads/governed-ai-2026.pdf'),
    ('The PCP-AI Competency Framework', 'PDF', 'PDF · 1.1 MB', '/downloads/pcp-ai-competency-framework.pdf'),
    ('Earned Value in AI-Assisted Schedules', 'PDF', 'PDF · 980 KB', '/downloads/evm-ai-schedules.pdf')]),
 'publications': ('Reports & publications', [
    ('Annual Report 2025', 'PDF', 'PDF · 3.2 MB', '/downloads/pci-annual-report-2025.pdf'),
    ('Pass-Rate & Standards Transparency Report', 'PDF', 'PDF · 640 KB', '/downloads/standards-transparency-2025.pdf'),
    ('Chapter Network Review', 'PDF', 'PDF · 1.8 MB', '/downloads/chapter-network-review.pdf')]),
}

# ---------------------------------------------------------------- 1. CSS out
style = soup.find('style')
os.makedirs(OUT + '/assets', exist_ok=True)
open(OUT + '/assets/styles.css', 'w', encoding='utf-8').write((style.string or '') + EXTRA_CSS)
CSS_BYTES = len(style.string or '')

# ---------------------------------------------------------------- 2. rewrite links in place (whole doc)
rewrote = 0
for a in soup.find_all('a'):
    dp   = a.get('data-page')
    href = a.get('href')
    target = None
    if dp is not None:
        target = dp
    elif href and href.startswith('#'):
        frag = href[1:]
        if frag in routeset or frag == 'home':
            target = frag
    if target is not None:
        a['href'] = route_path(target); rewrote += 1
    if 'data-page' in a.attrs:
        del a.attrs['data-page']

# ---------------------------------------------------------------- 2b. forms: make ready-to-wire (placeholder endpoints; real URLs pasted on the live site)
FORMS = {
 'contact': {'action': 'REPLACE_WITH_CONTACT_ENDPOINT', 'fields': [
     {'name': 'name', 'type': 'text', 'autocomplete': 'name', 'required': True},
     {'name': 'email', 'type': 'email', 'autocomplete': 'email', 'required': True},
     {'name': 'subject', 'type': 'text', 'required': True},
     {'name': 'message', 'required': True}]},
 'enrol': {'action': 'REPLACE_WITH_ENROL_ENDPOINT', 'fields': [
     {'name': 'first_name', 'type': 'text', 'autocomplete': 'given-name', 'required': True},
     {'name': 'last_name', 'type': 'text', 'autocomplete': 'family-name', 'required': True},
     {'name': 'email', 'type': 'email', 'autocomplete': 'email', 'required': True},
     {'name': 'country', 'type': 'text', 'autocomplete': 'country-name', 'required': True},
     {'name': 'background', 'required': True},
     {'name': 'experience', 'required': True}]},
 'login': {'action': 'REPLACE_WITH_LOGIN_ENDPOINT', 'fields': [
     {'name': 'email', 'type': 'email', 'autocomplete': 'email', 'required': True},
     {'name': 'password', 'type': 'password', 'autocomplete': 'current-password', 'required': True},
     {'name': 'remember', 'type': 'checkbox'}]},
 'forgot-password': {'action': 'REPLACE_WITH_FORGOT_ENDPOINT', 'fields': [
     {'name': 'email', 'type': 'email', 'autocomplete': 'email', 'required': True}]},
 'reset-password': {'action': 'REPLACE_WITH_RESET_ENDPOINT', 'fields': [
     {'name': 'password', 'type': 'password', 'autocomplete': 'new-password', 'required': True, 'minlength': '8'},
     {'name': 'password_confirm', 'type': 'password', 'autocomplete': 'new-password', 'required': True, 'minlength': '8'}]},
}
form_report = {}
for route, cfg in FORMS.items():
    sec = soup.find(id='p-' + route); form = sec.find('form')
    form['method'] = 'post'; form['action'] = cfg['action']
    form.insert_before(Comment(' TODO: set action="%s" to your live %s endpoint, then delete this note '
                               % (cfg['action'], route)))
    controls = [c for c in form.find_all(['input', 'textarea', 'select'])
                if not (c.name == 'input' and c.get('type') in ('submit', 'button'))]
    labels = form.find_all('label')
    assert len(controls) == len(cfg['fields']), (route, len(controls), len(cfg['fields']))
    for i, (ctl, fld) in enumerate(zip(controls, cfg['fields'])):
        if 'type' in fld and ctl.name == 'input': ctl['type'] = fld['type']
        ctl['name'] = fld['name']
        cid = ctl.get('id') or ('f-%s-%s' % (route, fld['name'])); ctl['id'] = cid
        if fld.get('required'):     ctl['required'] = ''
        if fld.get('autocomplete'): ctl['autocomplete'] = fld['autocomplete']
        if fld.get('minlength'):    ctl['minlength'] = fld['minlength']
        if i < len(labels) and not labels[i].get('for'): labels[i]['for'] = cid
    for b in form.find_all(['button', 'input']):            # prototype submit -> real submit
        cls = b.get('class') or []
        if 'protobtn' in cls:
            b['class'] = [c for c in cls if c != 'protobtn']; b['type'] = 'submit'
    if route == 'reset-password':                            # token from the reset link
        hid = soup.new_tag('input'); hid['type'] = 'hidden'; hid['name'] = 'token'; hid['id'] = 'resetToken'
        form.append(hid)
    def has_submit(f):
        return any((b.name in ('button', 'input')) and b.get('type') == 'submit' for b in f.find_all(['button', 'input']))
    if not has_submit(form):                                 # ensure a submit control exists
        for b in form.find_all('button'):
            c = b.get('class') or []
            if 'pw-toggle' in c or b.get('id') == 'googleBtn': continue
            b['type'] = 'submit'; break
    assert has_submit(form), 'no submit control in ' + route
    form_report[route] = (cfg['action'], [f['name'] for f in cfg['fields']])

# ---------------------------------------------------------------- 2c. thicken thin utility pages (instructions, timelines, privacy, support, what-happens-next)
def band(eyebrow, title, body, alt=False):
    cls = 'sec sec-2' if alt else 'sec'
    return ('<section class="%s"><div class="wrap"><span class="eyebrow">%s</span>'
            '<h2 style="margin:0 0 8px;font-size:clamp(26px,3vw,38px);">%s</h2>'
            '<div class="uline" style="margin-bottom:22px;"></div>%s</div></section>'
            % (cls, eyebrow, title, body))

def steps(items):
    return '<div class="steps">' + ''.join(
        '<div class="step"><div class="sn">%d</div><h3>%s</h3><p>%s</p></div>' % (i + 1, t, d)
        for i, (t, d) in enumerate(items)) + '</div>'

def facts(rows):
    return '<div class="facts">' + ''.join(
        '<div class="row"><div class="k">%s</div><div class="v">%s</div></div>' % (k, v)
        for k, v in rows) + '</div>'

def priv(kind):
    return ('<p style="max-width:78ch;color:var(--slate);margin-top:24px;"><strong>Your privacy.</strong> '
            'We use what you provide only to handle this %s, share it only with the people who need it to '
            'respond, and keep it no longer than necessary. See our <a href="/privacy">Privacy Policy</a> '
            'and <a href="/data-protection-policy">Data Protection Policy</a>.</p>' % kind)

SUPPORT = ('<p style="max-width:78ch;color:var(--slate);">Need help, or this in another format? '
           '<a href="/contact">Contact us</a> or email '
           '<a href="mailto:hello@projectcontrolsinstitute.org">hello@projectcontrolsinstitute.org</a> '
           'and we\u2019ll guide you through it.</p>')

CONTENT = {
 'contact': band('What to expect', 'After you get in touch.',
    steps([('You send a message', 'Use the form, or email us directly \u2014 whichever suits.'),
           ('We acknowledge', 'You\u2019ll get an automatic confirmation, usually within one working day.'),
           ('The right team replies', 'We route your message internally and respond, typically within two to three working days.'),
           ('We see it through', 'We stay with your query until it\u2019s resolved.')])
    + facts([('Response time', 'We aim to reply within two to three working days.'),
             ('Exam-day issues', 'Put \u201cexam day\u201d in the subject line for priority handling.'),
             ('Languages', 'We currently correspond in English.'),
             ('Accessibility', 'Need a call or another format? Tell us and we\u2019ll arrange it.')])
    + priv('enquiry') + SUPPORT, alt=True),

 'verify': band('How it works', 'Verifying a credential.',
    steps([('Enter the credential ID', 'You\u2019ll find it on the holder\u2019s digital badge or certificate.'),
           ('The registry responds', 'The lookup checks the ID against the PCI register of issued credentials.'),
           ('Read the status', 'A result shows as Valid, Expired or Revoked, with the holder\u2019s name as issued and the relevant dates.'),
           ('Can\u2019t find it?', 'It may be mistyped, withdrawn, or not yet issued \u2014 contact us to confirm.')])
    + facts([('What you can confirm', 'Name as issued, the credential, its status and dates.'),
             ('What stays private', 'No contact details, exam scores or personal data are shown.'),
             ('Employers', 'You can verify any PCP-AI holder, free, without an account.'),
             ('Reporting a concern', 'If a credential looks misrepresented, tell us and we\u2019ll review it.')])
    + priv('verification') + SUPPORT, alt=True),

 'enrol': band('After you register', 'What happens next.',
    steps([('We acknowledge', 'You\u2019ll hear back within one working day.'),
           ('We confirm your route', 'We check whether you qualify by experience or should begin with Foundation.'),
           ('We send your next steps', 'You\u2019ll receive candidate guidance and the key dates for the cohort.'),
           ('You prepare', 'Study with Certuvo\u2019s guides, practice banks and mock exams.'),
           ('You sit the exam', 'Book a proctored slot, online or at a centre, and earn your badge.')])
    + facts([('Acknowledgement', 'Within one working day.'),
             ('Eligibility decision', 'Usually within about five working days of a complete application.'),
             ('Cohort', 'Founding intake \u2014 dates are confirmed when you register.'),
             ('Fees &amp; funding', 'We set out fees and any employer or sponsor options when we contact you.'),
             ('Withdrawing', 'You can withdraw at any time before booking; fees follow the Refund Policy.')])
    + priv('application') + SUPPORT, alt=True),

 'request-info': band('What happens next', 'From enquiry to answer.',
    steps([('You tell us what you need', 'Certification details, corporate programmes, or academic partnership \u2014 whatever it is.'),
           ('We acknowledge', 'You\u2019ll get a reply confirming we\u2019ve received it, within two working days.'),
           ('The right team responds', 'Your enquiry reaches certification, corporate or academic colleagues as appropriate.'),
           ('We share what you asked for', 'Syllabus, fees, the roadmap, or a call \u2014 whatever helps you decide.')])
    + facts([('Response time', 'We aim to acknowledge within two working days.'),
             ('For organisations', 'Tell us your scale and goals for a tailored reply.'),
             ('Materials', 'Ask for the PCP-AI syllabus, fees or certification roadmap.'),
             ('Prefer to talk?', 'Ask for a call and we\u2019ll arrange a time.')])
    + priv('enquiry') + SUPPORT),

 'certification-application': band('How to apply', 'From application to exam.',
    steps([('Check your eligibility', 'Confirm you meet the experience route, or plan to begin with Foundation.'),
           ('Gather what you\u2019ll need', 'Have the details under \u201cWhat you\u2019ll need\u201d ready before you submit.'),
           ('Submit', 'Send your application via the contact form or by email, with your details.'),
           ('We assess eligibility', 'We acknowledge with a reference, then review your application.'),
           ('You book and sit the exam', 'Once approved, you schedule a proctored examination.')])
    + facts([('Acknowledgement', 'Within about three working days, with a reference number.'),
             ('Eligibility decision', 'Typically within about ten working days of a complete application.'),
             ('If something\u2019s missing', 'We\u2019ll tell you exactly what we need to proceed.'),
             ('Fees &amp; refunds', 'Examination fees and any refunds follow the Refund Policy.'),
             ('Reasonable adjustments', 'Need an accommodation? See Special Accommodations or just ask.')])
    + priv('application') + SUPPORT),

 'complaints-form': band('What happens next', 'How we handle your complaint.',
    steps([('You submit your complaint', 'Include what happened, when, and the outcome you\u2019re seeking.'),
           ('We acknowledge', 'You\u2019ll get confirmation with a reference, within about five working days.'),
           ('An impartial reviewer looks into it', 'Someone not involved in the matter examines it under the Complaints Policy.'),
           ('We share the outcome', 'You receive our findings and any action taken.'),
           ('Still unsatisfied?', 'You can escalate through the Appeals process.')])
    + facts([('Acknowledgement', 'Within about five working days, with a reference.'),
             ('Outcome', 'We aim to resolve within about thirty working days; we\u2019ll keep you informed if a complex case takes longer.'),
             ('Impartiality', 'Handled by someone not involved in the matter.'),
             ('Confidentiality', 'Treated in confidence; details are shared only as needed to investigate.'),
             ('Anonymity', 'You may ask to remain anonymous, though it can limit what we can do.')])
    + priv('complaint') + SUPPORT),

 'appeals-form': band('How an appeal works', 'Fair to challenge, fair to decide.',
    steps([('Lodge your appeal', 'Submit within the published window, stating the grounds clearly.'),
           ('We acknowledge', 'You\u2019ll get confirmation with a reference, within about five working days.'),
           ('Independent reviewers consider it', 'People not party to the original decision review whether it was handled correctly.'),
           ('We confirm the outcome', 'You receive the decision in writing, with reasons.'),
           ('The matter closes', 'The appeal decision is final for that stage of the process.')])
    + facts([('Window', 'Lodge within the published appeal window after a decision.'),
             ('Acknowledgement', 'Within about five working days, with a reference.'),
             ('Outcome', 'Typically within about twenty to thirty working days.'),
             ('Independence', 'Reviewed by people not involved in the original decision.'),
             ('Scope', 'An appeal checks the process was followed correctly \u2014 it is not an automatic re-mark.')])
    + priv('appeal') + SUPPORT),
}

for route, html_str in CONTENT.items():
    page = soup.find(id='p-' + route)
    frag = BeautifulSoup(html_str, 'lxml')
    nodes = list(frag.body.find_all(recursive=False))
    faq = page.find('details', class_='faq')
    anchor = faq.find_parent('section') if faq else page.find(class_='redband')
    for n in nodes:
        if anchor is not None: anchor.insert_before(n)
        else: page.append(n)

for route, (dt, files) in DOWNLOADS.items():
    page = soup.find(id='p-' + route)
    if page is None: continue
    frag = BeautifulSoup(dl_section(dt, files), 'lxml')
    anchor = page.find(class_='redband')
    for n in list(frag.body.find_all(recursive=False)):
        if anchor is not None: anchor.insert_before(n)
        else: page.append(n)

# ---------------------------------------------------------------- 3. shared chrome (serialize once, links already fixed)
skip   = str(soup.find('a', class_='skip'))
promo  = str(soup.find('div', class_='promo'))
header = str(soup.find('header', class_='hdr')).replace('<button aria-label="Search" class="ic">', '<button aria-label="Search" class="ic" id="siteSearchBtn">')
footer = str(soup.find('footer'))

def crumbbar(label):
    return ('<div class="crumbbar" id="crumbbar"><div class="wrap">'
            '<nav aria-label="Breadcrumb"><a href="/">Home</a>'
            '<span class="sep">&rsaquo;</span><span>' + H.escape(label) +
            '</span></nav></div></div>')

# ---------------------------------------------------------------- 4. scripts: keep interactive bits, drop the SPA router
def match_bracket(s, i, o, c):
    """i points at an opening bracket o; return index of its match, string-aware."""
    depth = 0; instr = False; esc = False; q = ''
    while i < len(s):
        ch = s[i]
        if instr:
            if esc: esc = False
            elif ch == '\\': esc = True
            elif ch == q: instr = False
        else:
            if ch in '"\'': instr = True; q = ch
            elif ch == o: depth += 1
            elif ch == c:
                depth -= 1
                if depth == 0: return i
        i += 1
    raise ValueError('no match')

def cut_after(text, anchor, openc, closec, paren_at=None):
    """Remove anchor..matching-bracket (+ trailing ';'). paren_at: literal preceding the bracket."""
    s = text.find(anchor)
    if s == -1: return text
    if paren_at is not None:
        oi = s + len(paren_at)
    else:
        oi = text.index(openc, s)
    e = match_bracket(text, oi, openc, closec)
    e += 1
    if e < len(text) and text[e] == ';': e += 1
    return text[:s] + text[e:]

js_scripts = [t for t in soup.find_all('script')
              if not t.get('src') and not t.get('type')]
router = next((t.string for t in js_scripts if t.string and 'function showPage' in t.string), '')
others = [t.string for t in js_scripts if t.string and 'function showPage' not in t.string]

keep = router
keep = cut_after(keep, 'var PMETA=', '{', '}')
keep = cut_after(keep, 'function setMeta(id)', '{', '}')
keep = cut_after(keep, 'function showPage(id)', '{', '}')
keep = cut_after(keep, "document.addEventListener('click',function(e){var a=e.target.closest('[data-page]')",
                 '(', ')', paren_at='document.addEventListener')
keep = cut_after(keep, "window.addEventListener('DOMContentLoaded',function(){var id='home';",
                 '(', ')', paren_at='window.addEventListener')
# drop the prototype contact/enrol handler (it faked a "your details were not sent" message)
keep = cut_after(keep, "document.addEventListener('click',function(e){var b=e.target.closest('.protobtn')",
                 '(', ')', paren_at='document.addEventListener')
# neutralise the donate button's fake "no payment was taken" message; keep the amount calculator
da = keep.find("closest('.ddonate');if(d)")
if da != -1:
    bo = keep.index('{', da + len("closest('.ddonate');if(d)"))
    bc = match_bracket(keep, bo, '{', '}')
    keep = keep[:bo] + '{/* TODO: send { amount: amt, frequency: freq } to your payment processor */}' + keep[bc + 1:]
keep = keep.strip()
assert 'showPage' not in keep and 'PMETA' not in keep and 'protobtn' not in keep, 'router/prototype not removed'
assert 'amt=100' in keep, 'lost the donate amount calculator'

# honest interaction helpers: password show/hide + reset match/token. No fake messages.
# (The original login/forgot/reset prototype script, with "not yet connected" copy, is intentionally dropped.)
AUTH_HELPERS = ("(function(){"
 "document.addEventListener('click',function(e){var t=e.target.closest('.pw-toggle');if(!t)return;"
 "var i=document.getElementById(t.getAttribute('data-target'));if(!i)return;"
 "var p=i.type==='password';i.type=p?'text':'password';t.textContent=p?'Hide':'Show';});"
 "var rf=document.getElementById('resetForm');if(rf){"
 "try{var tok=new URLSearchParams(location.search).get('token');var th=document.getElementById('resetToken');"
 "if(th&&tok)th.value=tok;}catch(e){}"
 "var a=document.getElementById('newPass'),b=document.getElementById('confirmPass');"
 "function chk(){if(a&&b)b.setCustomValidity(a.value!==b.value?'Passwords do not match':'');}"
 "if(a)a.addEventListener('input',chk);if(b)b.addEventListener('input',chk);}"
 "})();")

SCRIPTS = '<script>' + keep + '</script><script>' + AUTH_HELPERS + '</script><script>' + FEATURE_JS + '</script>'

# home-only: preserve old #hash deep links by redirecting to the real path
hashmap = '{' + ','.join('"%s":1' % r for r in routes if r not in ('home',)) + '}'
HASH_FALLBACK = ('<script>(function(){try{var R=' + hashmap +
                 ';var h=(location.hash||"").replace("#","");'
                 'if(h&&R[h]){location.replace("/"+h);}}catch(e){}})();</script>')

# ---------------------------------------------------------------- 5. structured data
def first_ldjson_graph():
    for t in soup.find_all('script', attrs={'type': 'application/ld+json'}):
        try: d = json.loads(t.string)
        except Exception: continue
        if isinstance(d, dict) and '@graph' in d: return d['@graph']
    return []

graph = first_ldjson_graph()
def node_of(types):
    for n in graph:
        t = n.get('@type'); t = t if isinstance(t, list) else [t]
        if any(x in t for x in types): return n
    return None

org  = node_of(['EducationalOrganization', 'NGO', 'Organization'])
site = node_of(['WebSite'])
cred = node_of(['EducationalOccupationalCredential'])
faq  = node_of(['FAQPage'])

# pull Course / ItemList / SearchAction from the standalone body ld+json blocks
course = itemlist = None
search_action = site.get('potentialAction') if site else None
for t in soup.find_all('script', attrs={'type': 'application/ld+json'}):
    try: d = json.loads(t.string)
    except Exception: continue
    if isinstance(d, dict):
        ty = d.get('@type')
        if ty == 'Course': course = d
        elif ty == 'ItemList': itemlist = d
        elif ty == 'WebSite' and d.get('potentialAction'): search_action = d['potentialAction']
if site is not None and search_action is not None:
    site = dict(site); site['potentialAction'] = search_action

shared_graph = [n for n in (org, site, cred) if n]

def ldjson(obj):
    return '<script type="application/ld+json">' + json.dumps(obj, ensure_ascii=False) + '</script>'

# ---------------------------------------------------------------- 6. <head> builder
def attr(v): return H.escape(v, quote=True)

def build_head(r, title, desc, canonical, label, noindex=False):
    ogtype = 'article' if r.startswith('blog-') else 'website'
    robots = 'noindex, follow' if noindex else 'index, follow, max-image-preview:large'
    parts = [
        '<meta charset="UTF-8"/>',
        '<meta name="viewport" content="width=device-width, initial-scale=1.0"/>',
        '<title>' + H.escape(title) + '</title>',
        '<meta name="description" content="' + attr(desc) + '"/>',
        '<meta name="keywords" content="' + attr(KEYWORDS) + '"/>',
        '<meta name="author" content="Project Controls Institute"/>',
        '<meta name="robots" content="' + robots + '"/>',
        '<meta name="theme-color" content="#D8261C"/>',
    ]
    if not noindex:
        parts.append('<link rel="canonical" href="' + attr(canonical) + '"/>')
    parts += [
        '<meta property="og:type" content="' + ogtype + '"/>',
        '<meta property="og:site_name" content="Project Controls Institute"/>',
        '<meta property="og:title" content="' + attr(title) + '"/>',
        '<meta property="og:description" content="' + attr(desc) + '"/>',
        '<meta property="og:url" content="' + attr(canonical) + '"/>',
        '<meta property="og:image" content="' + BASE + 'assets/og-image.jpg"/>',
        '<meta property="og:locale" content="en_GB"/>',
        '<meta name="twitter:card" content="summary_large_image"/>',
        '<meta name="twitter:title" content="' + attr(title) + '"/>',
        '<meta name="twitter:description" content="' + attr(desc) + '"/>',
        '<meta name="twitter:image" content="' + BASE + 'assets/og-image.jpg"/>',
        '<link rel="stylesheet" href="/assets/styles.css"/>',
    ]
    if shared_graph:
        parts.append(ldjson({'@context': 'https://schema.org', '@graph': shared_graph}))
    if not noindex:
        webpage = {'@context': 'https://schema.org', '@type': 'WebPage',
                   '@id': canonical + '#webpage', 'url': canonical, 'name': title,
                   'description': desc, 'isPartOf': {'@id': BASE + '#website'},
                   'inLanguage': 'en-GB'}
        page_nodes = [webpage]
        if r != 'home':
            webpage['breadcrumb'] = {'@id': canonical + '#breadcrumb'}
            page_nodes.append({'@context': 'https://schema.org', '@type': 'BreadcrumbList',
                '@id': canonical + '#breadcrumb', 'itemListElement': [
                    {'@type': 'ListItem', 'position': 1, 'name': 'Home', 'item': BASE},
                    {'@type': 'ListItem', 'position': 2, 'name': label, 'item': canonical}]})
        parts.append(ldjson(page_nodes if len(page_nodes) > 1 else webpage))
        if r == 'faq' and faq:        parts.append(ldjson(faq))
        if r == 'blog' and itemlist:  parts.append(ldjson(itemlist))
        if r in ('certification', 'curriculum') and course: parts.append(ldjson(course))
    return ''.join(parts)

# ---------------------------------------------------------------- 7. section html (cache, add active)
sec_html = {}
for s in sections:
    cls = s.get('class', [])
    if 'active' not in cls: cls = cls + ['active']
    s['class'] = cls
    sec_html[s['id'][2:]] = str(s)

# ---------------------------------------------------------------- 8. assemble + write pages
def page_doc(r, head, body_scripts, label):
    crumb = '' if r == 'home' else crumbbar(label)
    body = (skip + promo + header + crumb +
            '<main id="content">' + sec_html[r] + '</main>' +
            NEWSLETTER + footer + COOKIE + SEARCH_MODAL + body_scripts)
    return ('<!DOCTYPE html><html lang="en-GB"><head>' + head +
            '</head><body id="top">' + body + '</body></html>')

written = []
for r in routes:
    title, desc = pmeta.get(r, [r.replace('-', ' ').title() + ' | Project Controls Institute',
                                 'Project Controls Institute.'])
    label = title.split(' — ')[0].split(' | ')[0].strip()
    if r == '404':
        canonical = BASE + '404'
        head = build_head(r, title, desc, canonical, label, noindex=True)
        doc = page_doc(r, head, SCRIPTS, label)
        open(OUT + '/404.html', 'w', encoding='utf-8').write(doc); written.append('404.html')
    elif r == 'home':
        canonical = BASE
        head = build_head(r, title, desc, canonical, label)
        doc = page_doc(r, head, HASH_FALLBACK + SCRIPTS, label)
        open(OUT + '/index.html', 'w', encoding='utf-8').write(doc); written.append('index.html')
    else:
        canonical = BASE + r
        head = build_head(r, title, desc, canonical, label)
        doc = page_doc(r, head, SCRIPTS, label)
        open(OUT + '/' + r + '.html', 'w', encoding='utf-8').write(doc); written.append(r + '.html')

# ---------------------------------------------------------------- 9. sitemap / robots / host config
def prio(r):
    if r == 'home': return ('1.0', 'weekly')
    if r.startswith('blog-') or r.startswith('chapter-') or r.startswith('sector-'): return ('0.6', 'monthly')
    legal = ('policy', 'terms', 'privacy', 'cookie', 'disclaimer', 'copyright', 'accessibility')
    if any(k in r for k in legal): return ('0.4', 'yearly')
    return ('0.7', 'monthly')

urls = []
for r in routes:
    if r == '404': continue
    loc = BASE if r == 'home' else BASE + r
    p, cf = prio(r)
    urls.append('  <url><loc>%s</loc><lastmod>%s</lastmod><changefreq>%s</changefreq><priority>%s</priority></url>'
                % (loc, TODAY, cf, p))
sitemap = ('<?xml version="1.0" encoding="UTF-8"?>\n'
           '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">\n'
           + '\n'.join(urls) + '\n</urlset>\n')
open(OUT + '/sitemap.xml', 'w', encoding='utf-8').write(sitemap)

sidx = [{'u': route_path(r),
         't': pmeta.get(r, [r.replace('-', ' ').title()])[0].split(' | ')[0].split(' \u2014 ')[0].strip(),
         'd': (pmeta.get(r, ['', ''])[1] if len(pmeta.get(r, [''])) > 1 else '')}
        for r in routes if r != '404']
open(OUT + '/search-index.json', 'w', encoding='utf-8').write(json.dumps(sidx, ensure_ascii=False))

open(OUT + '/robots.txt', 'w', encoding='utf-8').write(
    'User-agent: *\nAllow: /\n\nSitemap: ' + BASE + 'sitemap.xml\n')

open(OUT + '/netlify.toml', 'w', encoding='utf-8').write(
    '[[redirects]]\n  from = "/*.html"\n  to = "/:splat"\n  status = 301\n  force = true\n\n'
    '[[redirects]]\n  from = "/*"\n  to = "/404.html"\n  status = 404\n')

open(OUT + '/_redirects', 'w', encoding='utf-8').write(
    '/*.html   /:splat     301\n/*        /404.html   404\n')

open(OUT + '/vercel.json', 'w', encoding='utf-8').write(
    json.dumps({'cleanUrls': True, 'trailingSlash': False}, indent=2) + '\n')

# ---------------------------------------------------------------- report
print('routes:', len(routes), '| html files written:', len(written))
print('links rewritten to real paths:', rewrote)
print('css externalised (bytes):', CSS_BYTES)
print('sitemap urls:', len(urls))
print('shared ld+json nodes:', [ (n.get("@type")) for n in shared_graph ])
print('keeper script bytes:', len(SCRIPTS))
