/* PCI CMS loader — plug-and-play bridge between the website and the PCI backend.
   Activation: set <meta name="pci-api" content="https://your-backend"> (site-wide).
   With no value set, this script does nothing and the site remains fully static. */
(function(){
  var BASE=(window.PCI_API_BASE||'').replace(/\/$/,'');
  /* Served by the PCI backend (the real deployment topology): the API is same-origin, so default to
     it when no explicit backend is configured. This makes the newsletter and the contact/corporate
     forms (which read window.PCI_API_BASE at submit time) and managed content work without pinning
     the <meta name="pci-api"> on all 200+ pages. A local file:// open (no server) stays fully static. */
  if(!BASE && (location.protocol==='http:'||location.protocol==='https:')) BASE=location.origin;
  if(!BASE) return;
  /* publish the resolved base so inline form handlers on this page pick up the same-origin default */
  window.PCI_API_BASE=BASE;
  var ctrl=('AbortController' in window)?new AbortController():null;
  if(ctrl) setTimeout(function(){ctrl.abort();},3000);
  fetch(BASE+'/api/content',{signal:ctrl&&ctrl.signal}).then(function(r){return r.json();}).then(function(d){
    window.PCI_CONTENT=d;
    var c=d.content||{};
    /* apply managed text to any element tagged data-cms (safe plain-text bindings) */
    document.querySelectorAll('[data-cms]').forEach(function(el){
      var k=el.getAttribute('data-cms');
      if(c[k]!=null && String(c[k]).trim()!=='' ){
        var cur=(el.textContent||'').replace(/\s+/g,' ').trim();
        var val=String(c[k]).replace(/\s+/g,' ').trim();
        if(cur!==val) el.textContent=String(c[k]);
      }
    });
    /* announcement banner */
    if(String(c.announcement_enabled)==='1' && c.announcement_text && !sessionStorage.getItem('pciannx')){
      var bar=document.createElement('div');
      bar.setAttribute('role','status');
      bar.style.cssText='background:#1D4ED8;color:#fff;font:600 13px/1.5 -apple-system,Segoe UI,Roboto,Arial,sans-serif;padding:10px 42px 10px 16px;text-align:center;position:relative;z-index:999';
      bar.textContent=c.announcement_text;
      var x=document.createElement('button');
      x.setAttribute('aria-label','Dismiss announcement');
      x.style.cssText='position:absolute;right:8px;top:50%;transform:translateY(-50%);background:none;border:none;color:#fff;font-size:16px;cursor:pointer;padding:4px 8px';
      x.textContent='\u00d7';
      x.onclick=function(){bar.remove();try{sessionStorage.setItem('pciannx','1');}catch(e){}};
      bar.appendChild(x);
      document.body.insertBefore(bar,document.body.firstChild);
    }
    /* newsletter form -> backend */
    var btn=document.getElementById('nlBtn'), em=document.getElementById('nlEmail');
    if(btn&&em){
      btn.addEventListener('click',function(){
        var v=(em.value||'').trim();
        if(!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(v)){ em.focus(); return; }
        btn.disabled=true;
        fetch(BASE+'/api/newsletter',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({email:v})})
          .then(function(){btn.textContent='Subscribed \u2713';})
          .catch(function(){btn.disabled=false;});
      });
    }
  }).catch(function(){/* backend unreachable: site stays fully static */});
})();
