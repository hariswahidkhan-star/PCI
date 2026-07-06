# Storage-abstraction logic mirror (validates the same rules the C# Storage helper enforces).
# The authoritative test is the C# runtime test; this mirror lets CI assert the rules without the SDK.
import base64, hashlib, os, tempfile

MAX=3_000_000
ALLOWED={'image/jpeg':'.jpg','image/png':'.png','image/webp':'.webp','application/pdf':'.pdf'}
MAGIC={'image/jpeg':b'\xff\xd8\xff','image/png':b'\x89PNG','application/pdf':b'%PDF'}

def decode(data_uri):
    if not data_uri or not data_uri.startswith('data:'): return None,None,'not_a_data_uri'
    try:
        head,b64=data_uri.split(',',1); mime=head[5:head.index(';')]
    except: return None,None,'malformed_data_uri'
    if mime not in ALLOWED: return None,mime,'file_type_not_allowed'
    try: raw=base64.b64decode(b64)
    except: return None,mime,'invalid_base64'
    if len(raw)>MAX: return None,mime,'file_too_large'
    if mime in MAGIC and not raw.startswith(MAGIC[mime]): return None,mime,'content_mime_mismatch'
    if mime=='image/webp' and not (raw[:4]==b'RIFF' and raw[8:12]==b'WEBP'): return None,mime,'content_mime_mismatch'
    return raw,mime,None

def put(root,raw,mime,cat):
    sha=hashlib.sha256(raw).hexdigest(); ext=ALLOWED.get(mime,'.bin')
    rel=f"{cat}/{sha[:2]}/{sha}{ext}"; full=os.path.join(root,rel)
    os.makedirs(os.path.dirname(full),exist_ok=True)
    if not os.path.exists(full): open(full,'wb').write(raw)
    return f"local:{rel}",sha,len(raw)

def get(root,ref):
    if not ref or ':' not in ref: return None
    rel=ref.split(':',1)[1]
    if '..' in rel or rel.startswith('/') or '\\' in rel: return None
    full=os.path.join(root,rel)
    return open(full,'rb').read() if os.path.exists(full) else None

def b64uri(raw,mime): return f"data:{mime};base64,{base64.b64encode(raw).decode()}"

print("=== STORAGE ABSTRACTION (metadata + reference model) ===\n")
root=tempfile.mkdtemp()
PNG=b'\x89PNG\r\n\x1a\n'+bytes(8); JPG=b'\xff\xd8\xff\xe0'+bytes(4); PDF=b'%PDF'+bytes(4)
p=0;f=0
def ck(n,ok):
    global p,f
    print(('PASS ' if ok else 'FAIL ')+n); p+= 1 if ok else 0; f+= 0 if ok else 1

raw,mime,err=decode(b64uri(PNG,'image/png')); ck("valid PNG decodes",err is None and raw==PNG)
ref,sha,size=put(root,raw,mime,'evidence')
ck("reference is provider-qualified, not a path",ref.startswith('local:evidence/') and '/tmp' not in ref and root not in ref)
ck("DB stores reference + metadata (not bytes)",len(sha)==64 and size==len(PNG))
ck("round-trip bytes match",get(root,ref)==PNG)
ref2,_,_=put(root,PNG,'image/png','evidence'); ck("content-addressed dedupe",ref2==ref)
_,_,e=decode('data:application/x-msdownload;base64,TVqQ'); ck("exe rejected",e=='file_type_not_allowed')
_,_,e=decode(b64uri(PNG,'application/pdf')); ck("content/MIME mismatch rejected",e=='content_mime_mismatch')
big=bytearray(MAX+50); big[0]=0xFF;big[1]=0xD8;big[2]=0xFF
_,_,e=decode(b64uri(bytes(big),'image/jpeg')); ck("oversized rejected",e=='file_too_large')
ck("path traversal refused",get(root,'local:../../etc/passwd') is None)
_,_,je=decode(b64uri(JPG,'image/jpeg')); _,_,pe=decode(b64uri(PDF,'application/pdf'))
ck("JPG + PDF accepted",je is None and pe is None)
print(f"\n{p} passing, {f} failing")
