const I18N={
  brandSub:['Operations Console','Consola de operaciones'],workspace:['PRIVATE WORKSPACE','ESPACIO PRIVADO'],commandCenter:['COMMAND CENTER','CENTRO DE CONTROL'],overview:['Overview','Resumen'],intake:['Realtor Intake','Solicitudes de Realtors'],approvals:['Pricing Approvals','Aprobaciones de precios'],relationships:['RELATIONSHIPS','RELACIONES'],realtors:['Realtor Partners','Realtors socios'],leads:['Lead Pipeline','Pipeline de leads'],clients:['Clients & Properties','Clientes y propiedades'],messages:['Messages','Mensajes'],operations:['OPERATIONS','OPERACIONES'],estimates:['Estimates','Estimados'],projects:['Projects','Proyectos'],assignments:['Crew Assignments','Asignaciones'],invoices:['Invoices & Payments','Invoices y pagos'],control:['CONTROL','CONTROL'],catalog:['Price Catalog','Catálogo de precios'],promotions:['Promotions','Promociones'],referrals:['Gifts & Referrals','Regalos y referidos'],integrations:['Integrations','Integraciones'],addRequest:['Add a property request','Agregar solicitud de propiedad'],propertyAddress:['Property address','Dirección de la propiedad'],source:['Source','Origen'],contact:['Contact','Contacto'],dueDate:['Due date','Fecha límite'],requestDetails:['What needs Management review?','¿Qué necesita revisar Management?'],cancel:['Cancel','Cancelar'],createRequest:['Create request','Crear solicitud'],
  gateTitle:['SAFE Management','SAFE Management'],gateIntro:['Create a profile, or sign in if you already have one.','Crea un perfil, o ingresa si ya tienes una cuenta.'],
  createProfile:['Create profile','Crear perfil'],signInExisting:['I already have an account','Ya tengo una cuenta'],fullName:['Full name','Nombre completo'],password:['Password','Contraseña'],
  haveAccount:['Already have an account? Sign in','¿Ya tienes cuenta? Ingresar'],needAccount:['No account yet? Create a profile','¿No tienes cuenta? Crear perfil'],
  badEmail:['Enter a complete email, such as name@company.com','Escribe un correo completo, como nombre@empresa.com'],shortPassword:['Use at least 6 characters.','Usa al menos 6 caracteres.'],
  accountExists:['That email already has a profile. Sign in.','Ese correo ya tiene un perfil. Ingresa.'],noAccount:['No profile matches that email and password.','No hay un perfil con ese correo y contraseña.'],
  profileReady:['Profile created. This is your account.','Perfil creado. Esta es tu cuenta.'],signedIn:['Signed in to your account.','Sesión iniciada en tu cuenta.'],signOut:['Sign out','Cerrar sesión'],signedOut:['Signed out','Sesión cerrada']
};
const state={route:location.hash.replace('#','')||'overview',previous:'overview',lang:localStorage.getItem('safe-management-lang')||'en',catalog:[],filter:'all',hub:{workOrders:[],messages:[],contacts:[],promotions:[],leads:[],threads:[],events:[],invoices:[],giftCards:[]},requests:[],field:{profiles:[],requests:[],activity:[]},localInvoices:[],realtorGifts:[]};
const el=id=>document.getElementById(id);
const t=(en,es)=>state.lang==='en'?en:es;
const esc=v=>String(v??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const money=v=>new Intl.NumberFormat('en-US',{style:'currency',currency:'USD',maximumFractionDigits:0}).format(Number(v)||0);
const initials=name=>String(name).split(/\s+/).map(x=>x[0]).slice(0,2).join('').toUpperCase();
const routeTitles={overview:['Operations Overview','Resumen de operaciones'],intake:['Realtor Intake','Solicitudes de Realtors'],approvals:['Pricing Approvals','Aprobaciones de precios'],realtors:['Realtor Partners','Realtors socios'],leads:['Lead Pipeline','Pipeline de leads'],clients:['Clients & Properties','Clientes y propiedades'],messages:['Messages','Mensajes'],estimates:['Estimates','Estimados'],projects:['Projects','Proyectos'],assignments:['Crew Assignments','Asignaciones'],invoices:['Invoices & Payments','Invoices y pagos'],catalog:['Price Catalog','Catálogo de precios'],promotions:['Promotions','Promociones'],referrals:['Gifts & Referrals','Regalos y referidos'],integrations:['Integrations','Integraciones']};
const moduleIndex=[
  ['overview','⌂','Overview dashboard operations today','Resumen operaciones hoy'],['intake','⇥','Realtor intake property request inspection','Solicitud Realtor propiedad inspección'],['approvals','✓','Pricing approvals estimates ranges','Aprobaciones precios estimados rangos'],['realtors','◎','Realtor partners brokerages','Realtors firmas inmobiliarias'],['leads','✦','Oscar lead pipeline Facebook Instagram','Pipeline leads Oscar Facebook Instagram'],['clients','◇','Clients properties addresses','Clientes propiedades direcciones'],['messages','◌','Messages Gmail SMS Facebook Instagram','Mensajes correo texto redes'],['estimates','▤','Estimates quotes options','Estimados cotizaciones opciones'],['projects','▦','Projects jobs work orders','Proyectos trabajos órdenes'],['assignments','↗','Crew assignments workers schedule','Asignaciones trabajadores agenda'],['invoices','$','Invoices payments receivables','Invoices pagos cobros'],['catalog','≡','Price catalog repairs construction','Catálogo precios reparaciones'],['promotions','%','Promotions offers campaigns','Promociones ofertas campañas'],['referrals','☆','Gift cards referrals commissions','Gift cards referidos comisiones'],['integrations','∞','Integrations API Gmail Meta Realtor portal','Integraciones API correo Meta portal Realtor']
];
function applyLanguage(){document.documentElement.lang=state.lang;document.querySelectorAll('[data-i18n]').forEach(n=>{const p=I18N[n.dataset.i18n];if(p)n.textContent=p[state.lang==='en'?0:1]});document.querySelectorAll('[data-placeholder-en]').forEach(n=>n.placeholder=state.lang==='en'?n.dataset.placeholderEn:n.dataset.placeholderEs);const next=state.lang==='en'?'ES':'EN';if(el('languageButton'))el('languageButton').textContent=next;if(el('mgmtLang'))el('mgmtLang').textContent=next}
function mgmtAccounts(){try{return JSON.parse(localStorage.getItem('safe-management-accounts')||'[]')}catch{return[]}}
function saveMgmtAccounts(list){localStorage.setItem('safe-management-accounts',JSON.stringify(list))}
function validEmail(value){return /^[^\s@]+@[^\s@]+\.[A-Za-z]{2,}$/.test(String(value||'').trim())}
function showMgmtGate(mode){
  state.loginMode=mode;
  const choices=el('mgmtChoices'),create=el('mgmtCreate'),sign=el('mgmtSignIn'),title=el('mgmtGateTitle');
  if(!choices||!create||!sign)return;
  choices.hidden=mode!=='choices';
  create.hidden=mode!=='create';
  sign.hidden=mode!=='signin';
  create.querySelectorAll('input').forEach(node=>node.disabled=mode!=='create');
  sign.querySelectorAll('input').forEach(node=>node.disabled=mode!=='signin');
  const key=mode==='create'?'createProfile':mode==='signin'?'signInExisting':'gateTitle';
  if(title&&I18N[key]){title.dataset.i18n=key;title.textContent=I18N[key][state.lang==='en'?0:1]}
}
function paintMgmt(){const account=state.account;if(!account||!el('mgmtInitial'))return;el('mgmtInitial').textContent=initials(account.name||'M')||'M'}
function enterMgmt(account){state.account=account;state.loggedIn=true;sessionStorage.setItem('safe-management-session',JSON.stringify({name:account.name,email:account.email}));document.body.classList.remove('needs-account');paintMgmt()}
function leaveMgmt(){state.account=null;state.loggedIn=false;sessionStorage.removeItem('safe-management-session');document.body.classList.add('needs-account');showMgmtGate('choices')}
function pageHead(kicker,en,es,descEn,descEs,actions=''){return `<header class="page-head"><div><span class="eyebrow">${kicker}</span><h1>${t(en,es)}</h1><p>${t(descEn,descEs)}</p></div><div class="page-actions">${actions}</div></header>`}
function apiError(body){return (state.lang==='es'&&body?.errorEs)||body?.error||''}
function isoDate(addDays){const d=new Date();d.setDate(d.getDate()+addDays);return `${d.getFullYear()}-${String(d.getMonth()+1).padStart(2,'0')}-${String(d.getDate()).padStart(2,'0')}`}
function mergeRequests(){state.requests=[...(state.localRequests||[]),...(state.hubRequests||[])]}
async function loadLocalRequests(){try{const res=await fetch('/api/operations/requests');if(res.ok){state.localRequests=await res.json();mergeRequests()}}catch{}}
async function loadLocalEstimates(){try{const res=await fetch('/api/operations/estimates');if(res.ok)state.localEstimates=await res.json()}catch{}}
function applySnap(snap){
  state.hub=snap;
  if(Array.isArray(snap.requests)){state.hubRequests=snap.requests;mergeRequests()}
  if(Array.isArray(snap.promotions))state.promotions=snap.promotions;
  if(Array.isArray(snap.leads))state.leads=snap.leads;
}
const contactCopy={Realtor:['Add Realtor partner','Agregar Realtor socio','Realtor saved','Realtor guardado'],Client:['Add client','Agregar cliente','Client saved','Cliente guardado'],Worker:['Add field worker','Agregar trabajador','Worker saved','Trabajador guardado']};
function ensureContactModal(){
  let m=el('contactModal');if(m)return m;
  m=document.createElement('div');m.className='modal';m.id='contactModal';m.setAttribute('role','dialog');m.setAttribute('aria-modal','true');
  m.onclick=e=>{if(e.target===m)m.classList.remove('open')};
  document.body.appendChild(m);return m;
}
function addContact(role){
  const m=ensureContactModal(),c=contactCopy[role]||contactCopy.Client;
  m.innerHTML=`<form class="modal-card" id="contactForm" novalidate><div class="modal-head"><div><span class="eyebrow">${role==='Realtor'?'PARTNER NETWORK':role==='Worker'?'FIELD TEAM':'CLIENT RECORDS'}</span><h2>${t(c[0],c[1])}</h2></div><button type="button" class="close" data-contact-close>×</button></div>
  <div class="form-grid"><label><span>${t('Full name','Nombre completo')} *</span><input id="contactName" required autocomplete="off"></label>${role==='Realtor'?`<label><span>${t('Realty firm','Firma inmobiliaria')}</span><input id="contactFirm" placeholder="${t('Brokerage name','Nombre de la firma')}"></label>`:role==='Client'?`<label><span>${t('Property address','Dirección de la propiedad')}</span><input id="contactAddress" placeholder="${t('Street, city, state','Calle, ciudad, estado')}"></label>`:`<label><span>${t('Trade','Oficio')}</span><input id="contactFirm" placeholder="${t('Electrician, plumber, general…','Electricista, plomero, general…')}"></label>`}<label><span>${t('Email','Correo')}</span><input id="contactEmail" type="email" placeholder="name@firm.com"></label><label><span>${t('Phone (10 digits)','Teléfono (10 dígitos)')}</span>${phoneField('contactPhone')}</label><label class="full"><span>${t('Notes','Notas')}</span><textarea id="contactNotes" rows="3"></textarea></label></div>
  <div class="form-warning" id="contactError" hidden></div>
  <div class="modal-actions"><button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit" id="contactSave">${t('Save','Guardar')}</button></div></form>`;
  m.querySelectorAll('[data-contact-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
  el('contactForm').onsubmit=e=>{e.preventDefault();saveContact(role)};bindPhone('contactPhone');
  m.classList.add('open');setTimeout(()=>el('contactName').focus(),50);
}
async function saveContact(role){
  const err=el('contactError'),btn=el('contactSave'),show=msg=>{err.textContent='⚠ '+msg;err.hidden=false};
  const payload={role,name:el('contactName').value.trim(),email:el('contactEmail').value.trim(),phone:el('contactPhone').value.trim(),firm:el('contactFirm')?.value.trim()||'',address:el('contactAddress')?.value.trim()||'',notes:el('contactNotes').value.trim()};
  err.hidden=true;
  if(payload.name.length<2){show(t('Write the full name.','Escribe el nombre completo.'));el('contactName').focus();return}
  if(payload.email&&!/^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$/.test(payload.email)){show(t('Write a valid email, like name@firm.com.','Escribe un correo válido, como nombre@firma.com.'));el('contactEmail').focus();return}
  if(payload.phone&&!phonePattern.test(payload.phone)){show(t('The phone must have exactly 10 digits, numbers only.','El teléfono debe tener exactamente 10 dígitos, solo números.'));el('contactPhone').focus();return}
  btn.disabled=true;btn.textContent=t('Saving…','Guardando…');
  try{
    const res=await fetch('/api/operations/contacts',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});
    const body=await res.json().catch(()=>({}));
    if(res.status===409){show(t(`${body.existing?.name||'This contact'} already exists (${body.existing?.publicId||''}).`,`${body.existing?.name||'Este contacto'} ya existe (${body.existing?.publicId||''}).`));return}
    if(!res.ok){show(apiError(body)||t('Could not save. Try again.','No se pudo guardar. Intenta de nuevo.'));return}
    state.localContacts=[...(state.localContacts||[]),body];
    el('contactModal').classList.remove('open');render();
    const c=contactCopy[role]||contactCopy.Client;toast(`✓ ${t(c[2],c[3])}: ${payload.name} (${body.publicId})`,5000);
  }catch{show(t('Could not save. Check your connection and try again.','No se pudo guardar. Revisa tu conexión e intenta de nuevo.'))}
  finally{btn.disabled=false;btn.textContent=t('Save','Guardar')}
}
function openLeadModal(){
  const m=ensureContactModal();
  m.innerHTML=`<form class="modal-card" id="leadForm" novalidate><div class="modal-head"><div><span class="eyebrow">OSCAR LEAD ENGINE</span><h2>${t('Add lead','Agregar lead')}</h2></div><button type="button" class="close" data-contact-close>×</button></div>
  <div class="form-grid"><label><span>${t('Full name','Nombre completo')} *</span><input id="leadName" required autocomplete="off"></label><label><span>${t('Realty firm','Firma inmobiliaria')}</span><input id="leadFirm" placeholder="${t('Brokerage name','Nombre de la firma')}"></label><label><span>${t('Email','Correo')}</span><input id="leadEmail" type="email" placeholder="name@firm.com"></label><label><span>${t('Phone (10 digits)','Teléfono (10 dígitos)')}</span>${phoneField('leadPhone')}</label><label><span>${t('Source','Origen')}</span><select id="leadChannel">${['Facebook','Instagram','Referral','Phone','Email','Event','Manual'].map(c=>`<option value="${c}"${c==='Manual'?' selected':''}>${t(c,{Referral:'Referido',Phone:'Teléfono',Email:'Correo',Event:'Evento'}[c]||c)}</option>`).join('')}</select></label><label><span>${t('Stage','Etapa')}</span><select id="leadStage">${leadStages.map(s=>`<option value="${s}">${t(...leadStageText[s])}</option>`).join('')}</select></label><label class="full"><span>${t('Notes and next step','Notas y próximo paso')}</span><textarea id="leadNotes" rows="3"></textarea></label></div>
  <div class="form-warning" id="leadError" hidden></div>
  <div class="modal-actions"><button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit" id="leadSave">${t('Save lead','Guardar lead')}</button></div></form>`;
  m.querySelectorAll('[data-contact-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
  el('leadForm').onsubmit=e=>{e.preventDefault();saveLead()};bindPhone('leadPhone');
  m.classList.add('open');setTimeout(()=>el('leadName').focus(),50);
}
async function saveLead(){
  const err=el('leadError'),btn=el('leadSave'),show=msg=>{err.textContent='⚠ '+msg;err.hidden=false};
  const payload={name:el('leadName').value.trim(),firm:el('leadFirm').value.trim(),email:el('leadEmail').value.trim(),phone:el('leadPhone').value.trim(),channel:el('leadChannel').value,stage:el('leadStage').value,notes:el('leadNotes').value.trim()};
  err.hidden=true;
  if(payload.name.length<2){show(t('Write the lead name.','Escribe el nombre del lead.'));el('leadName').focus();return}
  if(payload.email&&!/^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$/.test(payload.email)){show(t('Write a valid email, like name@firm.com.','Escribe un correo válido, como nombre@firma.com.'));el('leadEmail').focus();return}
  if(payload.phone&&!phonePattern.test(payload.phone)){show(t('The phone must have exactly 10 digits, numbers only.','El teléfono debe tener exactamente 10 dígitos, solo números.'));el('leadPhone').focus();return}
  btn.disabled=true;btn.textContent=t('Saving…','Guardando…');
  try{
    const res=await fetch('/api/operations/leads',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});
    const body=await res.json().catch(()=>({}));
    if(res.status===409){show(t(`${body.existing?.name||'This lead'} already exists (${body.existing?.publicId||''}, ${body.existing?.stage||''}).`,`${body.existing?.name||'Este lead'} ya existe (${body.existing?.publicId||''}, ${t(...(leadStageText[body.existing?.stage]||['','']))}).`));return}
    if(!res.ok){show(apiError(body)||t('Could not save. Try again.','No se pudo guardar. Intenta de nuevo.'));return}
    state.localLeads=[...(state.localLeads||[]),body];state.justAddedLead=body.publicId;
    el('contactModal').classList.remove('open');render();
    toast(`✓ ${t('Lead saved','Lead guardado')}: ${payload.name} (${body.publicId})`,5000);
  }catch{show(t('Could not save. Check your connection and try again.','No se pudo guardar. Revisa tu conexión e intenta de nuevo.'))}
  finally{btn.disabled=false;btn.textContent=t('Save lead','Guardar lead')}
}
async function moveLead(btn,publicId,stage){
  btn.disabled=true;
  try{
    const res=await fetch(`/api/operations/leads/${encodeURIComponent(publicId)}`,{method:'PATCH',headers:{'Content-Type':'application/json'},body:JSON.stringify({stage})});
    if(!res.ok)throw new Error();
    const lead=await res.json();
    state.localLeads=(state.localLeads||[]).map(l=>l.publicId===publicId?lead:l);state.justAddedLead=null;render();
    toast(`${lead.name} → ${t(...leadStageText[stage])}`);
  }catch{btn.disabled=false;toast(t('Could not move the lead. Try again.','No se pudo mover el lead. Intenta de nuevo.'))}
}
async function refreshHub(rerender=false){
  try{applySnap(await SafeApi.get('/api/safe/snapshot?portal=operations'));state.hubOnline=true;if(rerender)render();return true}
  catch(e){state.hubOnline=false;console.warn('IndorDB snapshot unavailable',e);return false}
}
async function getJson(url,fallback){try{const res=await fetch(url);if(res.ok)return await res.json()}catch{}return fallback}
async function loadField(){state.field=await getJson('/api/operations/field',state.field)}
async function loadLocalInvoices(){state.localInvoices=await getJson('/api/operations/invoices',state.localInvoices)}
async function loadRealtorGifts(){state.realtorGifts=await getJson('/api/operations/realtor-gift-cards',state.realtorGifts)}
function userTyping(){const a=document.activeElement;return !!(a&&el('main').contains(a)&&/^(INPUT|TEXTAREA|SELECT)$/.test(a.tagName))}
async function poll(){
  const before=JSON.stringify([state.hub,state.field]);
  await Promise.all([refreshHub(false),loadField()]);
  if(before!==JSON.stringify([state.hub,state.field])&&!userTyping())render();
}
async function postJson(url,payload,method='POST'){
  const res=await fetch(url,{method,headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});
  const body=await res.json().catch(()=>({}));
  if(!res.ok){const err=new Error(apiError(body)||t('Could not save. Try again.','No se pudo guardar. Intenta de nuevo.'));err.status=res.status;err.body=body;throw err}
  return body;
}
const phonePattern=/^\d{10}$/;
function phoneField(id){return `<input id="${id}" inputmode="numeric" maxlength="10" autocomplete="tel-national" placeholder="7045550100">`}
function bindPhone(id){const i=el(id);if(i)i.oninput=()=>{const v=i.value.replace(/\D/g,'').slice(0,10);if(v!==i.value)i.value=v}}
function busyButton(btn,text){const label=btn.textContent;btn.disabled=true;btn.textContent=text||t('Saving…','Guardando…');return()=>{btn.disabled=false;btn.textContent=label}}
function modalShell(id,eyebrow,title,body,actions){
  const m=ensureContactModal();
  m.innerHTML=`<form class="modal-card" id="${id}" novalidate><div class="modal-head"><div><span class="eyebrow">${eyebrow}</span><h2>${title}</h2></div><button type="button" class="close" data-contact-close>×</button></div>${body}<div class="form-warning" id="${id}Error" hidden></div><div class="modal-actions">${actions}</div></form>`;
  m.querySelectorAll('[data-contact-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
  m.classList.add('open');
  const show=msg=>{const e=el(id+'Error');e.textContent='⚠ '+msg;e.hidden=false};
  return {m,form:el(id),show};
}
function jobs(){return state.hub?.workOrders||[]}
function jobMessages(){return state.hub?.messages||[]}
function liveContacts(){
  const hub=state.hub?.contacts||[];
  const key=c=>`${c.role}|${(c.email||'').toLowerCase()||(c.name||'').toLowerCase()}`;
  const seen=new Set(hub.map(key));
  return [...hub,...(state.localContacts||[]).filter(c=>!seen.has(key(c)))];
}
async function loadLocalContacts(){try{const res=await fetch('/api/operations/contacts');if(res.ok)state.localContacts=await res.json()}catch{}}
function livePromos(){return state.hub?.promotions||[]}
function woStatusLabel(s){s=String(s||'pending');if(/arriv/i.test(s))return t('Arrived','Llegó');if(/progress/i.test(s))return t('In progress','En progreso');if(/assign/i.test(s))return t('Assigned','Asignado');if(/pending/i.test(s))return t('Needs crew','Necesita equipo');return s}
function toast(msg,ms=3200){const box=el('toast');box.textContent=msg;box.classList.add('show');clearTimeout(toast.timer);toast.timer=setTimeout(()=>box.classList.remove('show'),ms)}
function closeLayers(){el('sidebar').classList.remove('open');el('detailDrawer').classList.remove('open');el('detailDrawer').setAttribute('aria-hidden','true');el('overlay').classList.remove('open');el('menuToggle').setAttribute('aria-expanded','false')}
function navigate(route){if(route!==state.route)state.previous=state.route;state.route=route;location.hash=route;closeLayers();render();window.scrollTo({top:0,behavior:'smooth'});el('main').focus({preventScroll:true})}
function goBack(){if(state.route==='overview')return;navigate(state.previous&&state.previous!==state.route?state.previous:'overview')}
function metric(label,value,detail,warn=false){return `<article class="card metric"><span class="eyebrow">${label}</span><strong>${value}</strong><span class="delta ${warn?'warn':''}">${detail}</span></article>`}
const statusLabels={'new':['New','Nueva'],'ready to send':['Approved & sent','Aprobada y enviada'],'approved':['Approved','Aprobada'],'ready to assign':['Assigned to Operations','Asignada a Operaciones'],'review':['In review','En revisión'],'pricing review':['Pricing review','Revisión de precio'],'paid':['Paid','Pagado'],'active':['Active','Activo'],'pending':['Pending','Pendiente']};
function statusText(status){const l=statusLabels[String(status||'').toLowerCase()];return l?t(l[0],l[1]):String(status||'')}
function isApprovedRequest(r){return /ready to send|approved|sent/i.test(r?.status||'')}
function isAssignedRequest(r){return /ready to assign|assigned/i.test(r?.status||'')}
function statusTag(status){const cls=/approved|ready|paid|active|connected/i.test(status)?'green':/new|pricing|review|pending/i.test(status)?'gold':/late|blocked|risk/i.test(status)?'red':'aqua';return `<span class="tag ${cls}">${esc(statusText(status))}</span>`}
function priorityCell(p){return `<span class="priority"><i class="${p==='high'?'high':p==='low'?'low':''}"></i>${p[0].toUpperCase()+p.slice(1)}</span>`}
function requestTable(rows=state.requests){const body=rows.length?rows.map(r=>`<tr data-request="${r.id}"><td><div class="address">${esc(r.address)}<small>${esc(r.city)}</small></div></td><td>${esc(r.source)}</td><td>${esc(r.type)}</td><td>${statusTag(r.status)}</td><td>${esc(r.due)}</td><td>${esc(r.owner)}</td></tr>`).join(''):`<tr><td colspan="6">${t('No requests yet. Create one here or from the Realtor portal.','Aún no hay solicitudes. Créalas aquí o desde el portal del Realtor.')}</td></tr>`;return `<section class="card table-card"><div class="table-head"><div><span class="eyebrow">${t('LIVE WORK','TRABAJO ACTIVO')}</span><h2>${t('Requests moving through Management','Solicitudes pasando por Management')}</h2></div><button class="button secondary small" data-route="intake">${t('Open queue','Abrir cola')} →</button></div><table class="table"><thead><tr><th>${t('Property','Propiedad')}</th><th>${t('Source','Origen')}</th><th>${t('Request','Solicitud')}</th><th>${t('Status','Estado')}</th><th>${t('Due','Fecha')}</th><th>${t('Owner','Responsable')}</th></tr></thead><tbody>${body}</tbody></table></section>`}
const specialtyEs={'Electrical & Lighting':'Electricidad e iluminación','Roofing & Chimney':'Techos y chimenea','Exterior, Siding & Trim':'Exterior, siding y molduras','Gutters & Drainage':'Canaletas y drenaje','Doors, Windows & Garage':'Puertas, ventanas y garage','Plumbing':'Plomería','HVAC & Water Heater':'HVAC y calentador de agua','Interior, Drywall & Painting':'Interior, drywall y pintura','Flooring, Bathroom & Kitchen':'Pisos, baño y cocina','Crawlspace, Foundation & Structure':'Crawlspace, cimientos y estructura','Deck, Fence, Concrete & Outdoor':'Deck, cerca, concreto y exterior','Attic, Safety, Appliances & Cleaning':'Ático, seguridad, electrodomésticos y limpieza'};
function specialtyLabel(s){return state.lang==='es'?specialtyEs[s]||s:s}
function overviewView(){const urgent=state.requests.filter(r=>r.priority==='high').length;const open=state.requests.filter(r=>!isApprovedRequest(r));const next=open.find(r=>r.priority==='high')||open[0];const fieldPending=(state.field.requests||[]).filter(r=>r.status==='Pending').length;return `${pageHead('SAFE MANAGEMENT','Everything that needs a decision today','Todo lo que necesita una decisión hoy','Realtor requests, prices, projects, crews and client communication stay in one operating view.','Solicitudes, precios, proyectos, equipos y comunicación quedan en una sola vista.',`<button class="button secondary" data-route="integrations">∞ ${t('System health','Estado del sistema')}</button><button class="button primary" data-action="new-request">+ ${t('Add request','Agregar solicitud')}</button>`)}
  <section class="attention-panel"><div><span class="eyebrow">${t('MANAGEMENT PRIORITY','PRIORIDAD DE MANAGEMENT')}</span><h2>${urgent?t(`${urgent} high-priority requests need a decision`,`${urgent} solicitudes prioritarias necesitan decisión`):open.length?t(`${open.length} open request(s) waiting for a decision`,`${open.length} solicitud(es) abierta(s) esperan decisión`):t('No open requests yet','Aún no hay solicitudes')}</h2><p>${t('Realtor requests, crew assignments and job messages appear here from IndorDB.','Las solicitudes, asignaciones y mensajes aparecen aquí desde IndorDB.')}${fieldPending?' '+t(`${fieldPending} field team request(s) are waiting in Crew Assignments.`,`${fieldPending} solicitud(es) del equipo de campo esperan en Asignaciones.`):''}</p></div>${next?`<button class="button primary" data-request="${esc(next.id)}">${t('Open most urgent request','Abrir la solicitud más urgente')} →</button>`:''}</section>
  <div class="grid cols-4 section-gap">${metric(t('NEW REALTOR REQUESTS','SOLICITUDES NUEVAS'),state.requests.length,t('From IndorDB','Desde IndorDB'))}${metric(t('PRICING REVIEW','REVISAR PRECIOS'),state.requests.filter(r=>/pricing|review|new/i.test(r.status)).length,t('Need a decision','Necesitan decisión'),true)}${metric(t('WORKER ARRIVALS','LLEGADAS DE EQUIPO'),jobs().filter(j=>j.checkInAt).length,t('Realtor notified','Realtor notificado'))}${metric(t('ACTIVE PROJECTS','PROYECTOS ACTIVOS'),jobs().length,money(jobs().reduce((s,j)=>s+(Number(j.value)||0),0)))}</div>
  <section class="card section-gap"><div class="card-head"><div><span class="eyebrow">${t('CONNECTED WORKFLOW','FLUJO CONECTADO')}</span><h2>${t('One request, one timeline, three portals','Una solicitud, un timeline, tres portales')}</h2></div><span class="tag green">${t('Integration-ready','Listo para integrar')}</span></div><div class="flow-board"><div class="flow-stage active"><span>1</span><b>${t('Realtor sends request','Realtor envía solicitud')}</b><small>${t('Report, address, client and question','Reporte, propiedad, cliente y pregunta')}</small></div><div class="flow-stage"><span>2</span><b>${t('Management decides','Management decide')}</b><small>${t('Scope, price, priority and approval','Alcance, precio, prioridad y aprobación')}</small></div><div class="flow-stage"><span>3</span><b>${t('Client approves','Cliente aprueba')}</b><small>${t('Estimate, option or site visit','Estimado, opción o visita')}</small></div><div class="flow-stage"><span>4</span><b>${t('Crew receives work','Equipo recibe trabajo')}</b><small>${t('Arrival alerts Realtor + Management','Llegada avisa a Realtor + Management')}</small></div><div class="flow-stage"><span>5</span><b>${t('Status returns','Regresa el estado')}</b><small>${t('Thread, photos, invoice and update','Mensajes, fotos, invoice y actualización')}</small></div></div></section>
  <div class="section-gap">${requestTable(state.requests.slice(0,5))}</div>`}
function intakeView(){const counts={all:state.requests.length,new:state.requests.filter(r=>r.status==='New').length,pricing:state.requests.filter(r=>/pricing|review/i.test(r.status)).length,ready:state.requests.filter(r=>/ready/i.test(r.status)).length};const rows=state.filter==='all'?state.requests:state.requests.filter(r=>state.filter==='new'?r.status==='New':state.filter==='pricing'?/pricing|review/i.test(r.status):/ready/i.test(r.status));return `${pageHead('FROM REALTOR PORTAL','Review every new property request','Revisa cada solicitud nueva','Nothing goes to a crew until Management confirms the scope, price, owner and client response.','Nada pasa al equipo hasta que Management confirme alcance, precio, responsable y respuesta.',`<button class="button primary" data-action="new-request">+ ${t('Add request','Agregar solicitud')}</button>`)}<div class="queue-layout"><aside class="card filter-card"><span class="eyebrow">${t('QUEUE','COLA')}</span>${[['all',t('All requests','Todas'),counts.all],['new',t('New','Nuevas'),counts.new],['pricing',t('Needs review','Necesitan revisión'),counts.pricing],['ready',t('Ready','Listas'),counts.ready]].map(x=>`<button class="${state.filter===x[0]?'active':''}" data-filter="${x[0]}"><span>${x[1]}</span><b>${x[2]}</b></button>`).join('')}</aside><section class="request-list">${rows.length?rows.map(requestCard).join(''):`<article class="request-card"><p>${t('No requests in this queue.','No hay solicitudes en esta cola.')}</p></article>`}</section></div>`}
function requestCard(r){const done=isApprovedRequest(r);return `<article class="request-card${state.justCreated===r.id?' just-created':''}${done?' is-approved':''}"><div><div class="request-meta">${statusTag(r.status)}<span class="tag">${esc(r.source)}</span>${priorityCell(r.priority)}</div><h3>${esc(r.address)}</h3><p>${esc(r.type)} · ${r.items} ${t('items','items')} · ${esc(r.contact)}</p></div><div class="request-actions"><strong>${r.value?money(r.value):done?`✓ ${t('Approved','Aprobada')}`:isAssignedRequest(r)?t('Assigned','Asignada'):t('Needs review','Requiere revisión')}</strong><small>${esc(r.due)}</small><button class="button ${done?'secondary':'navy'} small" data-request="${r.id}">${done?t('View request','Ver solicitud'):t('Open request','Abrir solicitud')} →</button></div></article>`}
function approvalsView(){const rows=state.requests.filter(r=>/pricing|review|new/i.test(r.status));return `${pageHead('PRICE CONTROL','Approve what the Realtor will see','Aprueba lo que verá el Realtor','Compare the repair catalog, previous accepted work and grouped crew-day pricing before sending the range.','Compara catálogo, trabajos aceptados y paquetes por día antes de enviar el rango.')}<div class="grid cols-3">${metric(t('AWAITING REVIEW','ESPERANDO REVISIÓN'),rows.length,t('From IndorDB','Desde IndorDB'))}${metric(t('CATALOG ITEMS','ÍTEMS DE CATÁLOGO'),state.catalog.length,t('Shared price list','Lista de precios'))}${metric(t('OPEN REQUESTS','SOLICITUDES ABIERTAS'),state.requests.length,t('All statuses','Todos los estados'))}</div><div class="section-gap">${requestTable(rows)}</div>`}
function realtorsView(){const rows=liveContacts().filter(c=>c.role==='Realtor');const body=rows.length?rows.map(c=>{const reqs=state.requests.filter(r=>r.contact===c.name);const val=reqs.reduce((s,r)=>s+(Number(r.value)||0),0);return `<tr><td><div class="person"><span class="avatar">${initials(c.name)}</span><div><strong>${esc(c.name)}</strong>${c.email||c.phone?`<small class="person-sub">${esc([c.email,c.phone].filter(Boolean).join(' · '))}</small>`:''}</div></div></td><td>${esc(c.firm||'—')}</td><td>${liveContacts().filter(x=>x.role==='Client').length}</td><td>${reqs.length}</td><td><strong>${val?money(val):'—'}</strong></td></tr>`}).join(''):`<tr><td colspan="5">${t('No Realtors yet. Use “Add Realtor” to create the first partner.','Aún no hay Realtors. Usa “Agregar Realtor” para crear el primer socio.')}</td></tr>`;return `${pageHead('PARTNER NETWORK','Realtors, brokerages and activity','Realtors, firmas y actividad','See who sent each opportunity, what is active and what communication needs a response.','Mira quién envió cada oportunidad, qué está activo y qué comunicación necesita respuesta.',`<button class="button primary" data-add-contact="Realtor">+ ${t('Add Realtor','Agregar Realtor')}</button>`)}<section class="card table-card"><table class="table"><thead><tr><th>${t('Partner','Socio')}</th><th>${t('Firm','Firma')}</th><th>${t('Clients','Clientes')}</th><th>${t('Open requests','Solicitudes')}</th><th>${t('Project value','Valor')}</th></tr></thead><tbody>${body}</tbody></table></section>`}
const leadStages=['New','Qualified','Conversation','Demo'];
const leadStageText={New:['New','Nuevo'],Qualified:['Qualified','Calificado'],Conversation:['Conversation','Conversación'],Demo:['Demo','Demo']};
function liveLeads(){
  const hub=state.hub?.leads||[];
  const key=l=>(l.email||'').toLowerCase()||(l.name||'').toLowerCase();
  const seen=new Set(hub.map(key));
  return [...hub,...(state.localLeads||[]).filter(l=>!seen.has(key(l)))];
}
async function loadLocalLeads(){try{const res=await fetch('/api/operations/leads');if(res.ok)state.localLeads=await res.json()}catch{}}
function leadCard(x){
  const i=leadStages.indexOf(x.stage),next=leadStages[i+1];
  const meta=[x.firm,x.channel].filter(Boolean).map(esc).join(' · ');
  const contact=[x.email,x.phone].filter(Boolean).map(esc).join(' · ');
  return `<article class="pipeline-card${x.publicId&&x.publicId===state.justAddedLead?' just-created':''}"><strong>${esc(x.name)}</strong>${meta?`<p>${meta}</p>`:''}${contact?`<p class="lead-contact">${contact}</p>`:''}${x.notes?`<p>${esc(x.notes)}</p>`:''}${x.source==='local'?`<div class="lead-actions"><small>${esc(x.publicId)}</small>${i>0?`<button class="link-button" data-lead-move="${esc(x.publicId)}" data-stage="${leadStages[i-1]}">← ${t(...leadStageText[leadStages[i-1]])}</button>`:''}${next?`<button class="link-button" data-lead-move="${esc(x.publicId)}" data-stage="${next}">${t(...leadStageText[next])} →</button>`:''}</div>`:''}</article>`;
}
function leadsView(){const live=liveLeads();const stages=leadStages;const grouped=stages.map(s=>[s,live.filter(l=>String(l.stage||'New').toLowerCase().startsWith(s.toLowerCase().slice(0,3)))]);const pipeline=grouped.map((c,i)=>`<section class="pipeline-column"><h3>${t(...leadStageText[c[0]])}<span>${c[1].length}</span></h3>${(c[1].length?c[1].map(leadCard):[`<article class="pipeline-card"><strong>${t('Empty','Vacío')}</strong><p>${t('Add a lead from this page.','Agrega un lead desde esta página.')}</p></article>`]).join('')}</section>`).join('');return `${pageHead('OSCAR LEAD ENGINE','From social activity to a qualified Realtor','De actividad social a Realtor calificado','Management reviews algorithm leads before any outreach. One record holds source, consent, message and next action.','Management revisa los leads antes de contactar. Un registro guarda origen, permiso, mensaje y próximo paso.',`<button class="button primary" data-action="new-lead">+ ${t('Add lead','Agregar lead')}</button>`)}<div class="pipeline">${pipeline}</div>`}
function clientsView(){const rows=liveContacts().filter(c=>c.role==='Client');const props=state.hub?.properties||[];const body=rows.length?rows.map(c=>{const p=props.find(x=>x.clientId===c.id);const job=jobs().find(j=>j.client===c.name);return `<tr><td><div class="person"><span class="avatar">${initials(c.name)}</span><div><strong>${esc(c.name)}</strong>${c.email||c.phone?`<small class="person-sub">${esc([c.email,c.phone].filter(Boolean).join(' · '))}</small>`:''}</div></div></td><td>${esc(p?.address||job?.address||c.address||'—')}</td><td>${statusTag(job?.status||p?.status||'Active')}</td><td><strong>${job?.value?money(job.value):'—'}</strong></td></tr>`}).join(''):`<tr><td colspan="4">${t('No clients yet. Use “Add client” to create the first one.','Aún no hay clientes. Usa “Agregar cliente” para crear el primero.')}</td></tr>`;return `${pageHead('CLIENT RECORDS','Clients, properties and every connected job','Clientes, propiedades y trabajos conectados','The Realtor relationship and SAFE project remain visible without exposing internal crew costs to the Realtor portal.','La relación del Realtor y el proyecto quedan visibles sin mostrar costos internos.',`<button class="button primary" data-add-contact="Client">+ ${t('Add client','Agregar cliente')}</button>`)}<section class="card table-card"><table class="table"><thead><tr><th>${t('Client','Cliente')}</th><th>${t('Property','Propiedad')}</th><th>${t('Stage','Etapa')}</th><th>${t('Approved value','Valor aprobado')}</th></tr></thead><tbody>${body}</tbody></table></section>`}
function fieldWorkers(){
  const map=new Map();
  (state.field.profiles||[]).forEach(p=>map.set(p.email,{key:p.email,name:p.name,company:p.company,phone:p.phone,specialties:p.specialties||[],profile:true}));
  (state.field.activity||[]).filter(a=>a.kind!=='reply').forEach(a=>{if(!map.has(a.worker))map.set(a.worker,{key:a.worker,name:a.workerName||a.worker,specialties:[]})});
  (state.field.requests||[]).forEach(r=>{if(!map.has(r.worker))map.set(r.worker,{key:r.worker,name:r.workerName||r.worker,specialties:[]})});
  return [...map.values()];
}
function fieldThread(key){return (state.field.activity||[]).filter(a=>a.worker===key).slice().reverse()}
const fieldKindText={checkin:['Arrival','Llegada'],access:['Access request','Solicitud de acceso'],photos:['Photos','Fotos'],change:['Change request','Solicitud de cambio']};
const fieldDetailText={lockbox:['Lockbox code','Código del lockbox'],gate:['Gate or garage code','Código de reja o garage'],owner:['Owner or tenant to open','Que abra el dueño o inquilino'],other:['Other instructions','Otras instrucciones'],before:['Before','Antes'],during:['During','Durante'],after:['After','Después'],material:['Material','Material'],extra:['Extra work','Trabajo extra'],scope:['Scope change','Cambio de alcance']};
function whenText(v){return new Date(v).toLocaleString(state.lang==='es'?'es-US':'en-US',{dateStyle:'short',timeStyle:'short'})}
function fieldDetail(a){
  if(a.kind==='checkin'){const at=new Date(a.detail);return isNaN(at)?'':t('arrived ','llegó ')+whenText(at)}
  const [key,...rest]=String(a.detail||'').split(' · ');const label=fieldDetailText[key];
  return [label?t(...label):key,...rest].filter(Boolean).join(' · ');
}
function fieldEntry(a){
  if(a.kind==='reply')return `<div class="chat-bubble mine"><p>${esc(a.body)}</p><small>${esc(a.workerName||'Management')} · ${esc(whenText(a.createdAt))}</small></div>`;
  if(a.kind==='message')return `<div class="chat-bubble"><p>${esc(a.body)}</p><small>${esc(whenText(a.createdAt))}</small></div>`;
  const detail=fieldDetail(a);
  return `<div class="chat-event"><strong>${esc(t(...(fieldKindText[a.kind]||[a.kind,a.kind])))}</strong>${detail?` · ${esc(detail)}`:''}<span>${esc(a.address)}</span>${a.body?`<p>${esc(a.body)}</p>`:''}${a.photoCount?`<button type="button" class="link-button" data-field-photos="${a.id}">${t(`View ${a.photoCount} photo(s)`,`Ver ${a.photoCount} foto(s)`)}</button>`:''}<small>${esc(whenText(a.createdAt))}</small></div>`;
}
function messagesView(){
  const workers=fieldWorkers();
  if(!workers.some(w=>w.key===state.fieldChat))state.fieldChat=(workers.find(w=>fieldThread(w.key).length)||workers[0])?.key;
  const current=workers.find(w=>w.key===state.fieldChat);
  const waiting=w=>{const th=fieldThread(w.key),last=th[th.length-1];return last&&last.kind!=='reply'};
  const people=workers.length?workers.map(w=>`<button type="button" class="chat-person ${w.key===state.fieldChat?'active':''}" data-field-chat="${esc(w.key)}"><span class="avatar">${initials(w.name||'?')}</span><span><strong>${esc(w.name)}</strong><small>${esc(w.company||w.key)}</small></span>${waiting(w)?'<i class="dot" title="'+t('Waiting for reply','Esperando respuesta')+'"></i>':''}</button>`).join(''):`<p class="muted-note">${t('No field team activity yet. When a worker writes, checks in or asks for access, it appears here.','Aún no hay actividad del equipo de campo. Cuando un trabajador escriba, marque llegada o pida acceso, aparece aquí.')}</p>`;
  const thread=current?fieldThread(current.key):[];
  const chat=current?`<div class="chat-log" id="fieldChatLog">${thread.length?thread.map(fieldEntry).join(''):`<p class="muted-note">${t('No messages yet.','Aún no hay mensajes.')}</p>`}</div><div class="chat-compose"><textarea id="fieldReply" rows="2" maxlength="2000" placeholder="${esc(t(`Reply privately to ${current.name}…`,`Responder en privado a ${current.name}…`))}"></textarea><button type="button" class="button primary" data-action="field-reply">${t('Send','Enviar')}</button></div>`:'';
  const recipients=liveContacts().filter(c=>(c.role==='Realtor'||c.role==='Client')&&c.email);
  const email=`<form id="emailForm" class="form-grid flush" novalidate><label class="full"><span>${t('To','Para')} *</span><select id="emailTo">${recipients.length?recipients.map(c=>`<option value="${esc(c.email)}">${esc(c.name)} · ${c.role==='Realtor'?'Realtor':t('Client','Cliente')} · ${esc(c.email)}</option>`).join(''):`<option value="">${t('Add a Realtor or client with an email first','Primero agrega un Realtor o cliente con correo')}</option>`}</select></label><label class="full"><span>${t('Subject','Asunto')} *</span><input id="emailSubject" maxlength="200"></label><label class="full"><span>${t('Message','Mensaje')} *</span><textarea id="emailBody" rows="4" maxlength="20000"></textarea></label><div class="full"><button class="button primary" type="submit" id="emailSend">${t('Send email','Enviar correo')}</button></div></form>`;
  const job=jobs()[0];
  return `${pageHead('UNIFIED COMMUNICATIONS','Every conversation with the field team, Realtors and clients','Todas las conversaciones con el equipo, Realtors y clientes','Field workers write privately from their portal. Realtors and clients receive email from the company address.','Los trabajadores escriben en privado desde su portal. Realtors y clientes reciben correo desde la cuenta de la compañía.')}
  <div class="grid cols-2"><section class="card"><div class="card-head"><div><span class="eyebrow">${t('FIELD TEAM · PRIVATE','EQUIPO DE CAMPO · PRIVADO')}</span><h2>${current?esc(current.name):t('Field team messages','Mensajes del equipo de campo')}</h2></div>${current?.phone?`<span class="tag">${esc(current.phone)}</span>`:''}</div><div class="chat-layout"><div class="chat-people">${people}</div><div class="chat-main">${chat}</div></div></section>
  <section class="card"><div class="card-head"><div><span class="eyebrow">${t('REALTORS & CLIENTS','REALTORS Y CLIENTES')}</span><h2>${t('Write to a Realtor or client','Escribir a un Realtor o cliente')}</h2></div><span class="tag ${state.emailReady?'green':'gold'}">${state.emailReady?t('Email ready','Correo listo'):t('Email not configured','Correo sin configurar')}</span></div>${email}</section></div>
  ${job?jobThreadSection(job):''}`;
}
function jobThreadSection(job){const list=jobMessages();const bubbles=list.length?list.map(m=>`<div class="detail-block"><strong>${esc(m.senderName)} · ${esc(m.senderRole)}</strong><p>${esc(m.body)}</p></div>`).join(''):`<p>${t('No job messages yet. When a worker checks in, it appears here.','Aún no hay mensajes. Cuando un trabajador marque llegada, aparece aquí.')}</p>`;return `<div class="grid cols-2 section-gap"><section class="card"><div class="card-head"><div><span class="eyebrow">SAFE JOB THREAD</span><h2>${esc(job?.address||t('No active job','Sin trabajo activo'))}</h2></div><span class="tag green">${esc(woStatusLabel(job?.status))}</span></div><p>${esc(job?.id||'—')} · ${esc(job?.threadId||'')} · ${esc(job?.worker||'')}</p><div class="section-gap">${bubbles}</div><div class="detail-block"><input id="opsMsgInput" placeholder="${t('Reply to worker and Realtor…','Responder al trabajador y Realtor…')}" style="width:100%;margin-bottom:8px"><button class="button primary" data-action="ops-send">${t('Send to shared thread','Enviar a la conversación')}</button></div><button class="button secondary" data-action="acknowledge-arrival">${t('Acknowledge and monitor','Confirmar y supervisar')}</button></section><section class="card"><div class="card-head"><div><span class="eyebrow">${t('WORKER TRACKING','SEGUIMIENTO DE EQUIPO')}</span><h2>${t('Live field status','Estado en campo')}</h2></div></div>${jobs().slice(0,6).map(j=>`<div class="connection-row"><div><strong>${esc(j.address||j.id)}</strong><small>${esc(j.worker)} · ${esc(woStatusLabel(j.status))} · ${j.progress||0}%</small>${j.workerId?`<small class="access-line" id="ops-access-${esc(j.id)}"></small>`:''}</div><span class="row-actions"><span class="tag">${esc(j.id||'')}</span>${j.workerId?`<button type="button" class="button secondary small" data-view-access="${esc(j.id)}">${t('View access','Ver acceso')}</button>`:''}</span></div>`).join('')||`<p>${t('Assign a request to see workers here.','Asigna una solicitud para ver trabajadores aquí.')}</p>`}<button class="button secondary" data-route="assignments">${t('Open assignments','Abrir asignaciones')}</button></section></div>`}
function invoiceFor(estimateId){return (state.localInvoices||[]).find(i=>i.estimateId===estimateId&&i.status!=='Void')}
function projectsView(){
  const approved=(state.localEstimates||[]).filter(e=>e.status==='Approved');
  const estRows=approved.length?approved.map(e=>{const inv=invoiceFor(e.publicId);return `<tr><td><strong>${esc(e.publicId)}</strong>${e.requestId?`<small class="person-sub">${esc(e.requestId)}</small>`:''}</td><td><strong>${esc(e.address)}</strong><small class="person-sub">${esc(e.client)}</small></td><td>${(e.lines||[]).map(l=>esc(l.service)).join(', ')}</td><td><strong>${money(e.total)}</strong></td><td>${inv?`${invoiceStatusTag(inv.status)}<small class="person-sub">${esc(inv.publicId)}</small>`:`<button class="button primary small" data-invoice-for="${esc(e.publicId)}">${t('Create invoice','Crear invoice')}</button>`}</td></tr>`}).join(''):`<tr><td colspan="5">${t('No approved estimates yet. Mark an estimate as approved in Estimates to start a project.','Aún no hay estimados aprobados. Marca un estimado como aprobado en Estimados para iniciar un proyecto.')}</td></tr>`;
  const rows=jobs();
  const woRows=rows.length?rows.map(j=>`<tr><td class="address">${esc(j.address||'—')}</td><td>${esc(j.scope||'—')}</td><td><select class="inline-select" data-wo-progress="${esc(j.id)}" aria-label="${t('Progress','Progreso')}">${[0,25,50,75,100].map(p=>`<option value="${p}"${Number(j.progress||0)===p?' selected':''}>${p}%</option>`).join('')}</select><small class="person-sub">${esc(woStatusLabel(j.status))}</small></td><td>${j.value?money(j.value):'—'}</td><td>${esc(j.worker||t('Unassigned','Sin asignar'))}</td></tr>`).join(''):`<tr><td colspan="5">${t('No work orders yet. A Realtor request creates one automatically.','Aún no hay órdenes. Una solicitud del Realtor crea una sola.')}</td></tr>`;
  return `${pageHead('PROJECT DELIVERY','Approved work from scheduling to completion','Trabajo aprobado hasta completar','Approved estimates become projects. Update progress and bill them from here.','Los estimados aprobados se vuelven proyectos. Actualiza el progreso y factúralos desde aquí.',`<button class="button secondary" data-route="estimates">${t('Open estimates','Abrir estimados')}</button>`)}
  <section class="card table-card"><div class="table-head"><div><span class="eyebrow">${t('APPROVED ESTIMATES','ESTIMADOS APROBADOS')}</span><h2>${t('Projects ready to deliver and bill','Proyectos listos para ejecutar y facturar')}</h2></div></div><table class="table"><thead><tr><th>${t('Estimate','Estimado')}</th><th>${t('Property','Propiedad')}</th><th>${t('Services','Servicios')}</th><th>${t('Total','Total')}</th><th>Invoice</th></tr></thead><tbody>${estRows}</tbody></table></section>
  <section class="card table-card section-gap"><div class="table-head"><div><span class="eyebrow">${t('WORK ORDERS','ÓRDENES DE TRABAJO')}</span><h2>${t('Field progress','Progreso en campo')}</h2></div></div><table class="table"><thead><tr><th>${t('Property','Propiedad')}</th><th>${t('Scope','Alcance')}</th><th>${t('Progress','Progreso')}</th><th>${t('Value','Valor')}</th><th>${t('Assignment','Asignación')}</th></tr></thead><tbody>${woRows}</tbody></table></section>`;
}
const fieldStatusText={Pending:['Pending','Pendiente','gold'],Reviewed:['Reviewed','Revisada','aqua'],Approved:['Approved','Aprobada','green'],Declined:['Declined','Rechazada','red'],Converted:['In Management','En Management','green']};
function fieldStatusTag(s){const x=fieldStatusText[s]||[s,s,''];return `<span class="tag ${x[2]}">${esc(t(x[0],x[1]))}</span>`}
function assignmentsView(){
  const workers=fieldWorkers();
  const team=workers.length?workers.map(w=>`<article class="card worker-card"><div class="person"><span class="avatar">${initials(w.name||'?')}</span><div><strong>${esc(w.name)}</strong><small class="person-sub">${esc([w.company,w.phone].filter(Boolean).join(' · ')||w.key)}</small></div></div>${w.specialties?.length?`<div class="chips">${w.specialties.map(s=>`<span class="tag aqua">${esc(specialtyLabel(s))}</span>`).join('')}</div>`:`<p class="muted-note">${t('No specialties in the profile yet.','Aún no tiene especialidades en el perfil.')}</p>`}<div class="row-actions start"><button class="button secondary small" data-chat-with="${esc(w.key)}">${t('Message','Mensaje')}</button></div></article>`).join(''):`<article class="card"><p>${t('No field workers yet. They appear when they create a profile in the Field Team portal.','Aún no hay trabajadores. Aparecen cuando crean su perfil en el portal del equipo de campo.')}</p></article>`;
  const reqs=state.field.requests||[];
  const reqRows=reqs.length?reqs.map(r=>{const open=r.status==='Pending'||r.status==='Reviewed';const services=r.kind==='new_property'?t('New property','Nueva propiedad'):((r.items||[]).map(i=>`${esc(i.repairName)} · <b>${money(i.price)}</b>`).join('<br>')||esc(r.repairName));return `<tr><td><strong>${esc(r.workerName||r.worker)}</strong><small class="person-sub">${esc(whenText(r.createdAt))}</small></td><td><strong>${esc(r.address)}</strong>${r.clientName||r.clientPhone?`<small class="person-sub">${esc([r.clientName,r.clientPhone].filter(Boolean).join(' · '))}</small>`:''}</td><td>${services}${r.notes?`<small class="person-sub">${esc(r.notes)}</small>`:''}</td><td><strong>${r.total?money(r.total):'—'}</strong></td><td>${fieldStatusTag(r.status)}</td><td><div class="row-actions">${open?`<button class="button primary small" data-field-convert="${r.id}">${t('Send to intake','Pasar a solicitudes')}</button><button class="link-button" data-field-status="${r.id}" data-status="Declined">${t('Decline','Rechazar')}</button>`:''}</div></td></tr>`}).join(''):`<tr><td colspan="6">${t('No requests from the field team yet.','Aún no hay solicitudes del equipo de campo.')}</td></tr>`;
  const crew=(state.hub?.contacts||[]).filter(c=>c.role==='Worker');
  const cards=jobs();
  const orders=cards.length?cards.map(j=>`<article class="card"><span class="tag ${/arriv/i.test(j.status)?'green':/pending/i.test(j.status)?'gold':'aqua'}">${esc(woStatusLabel(j.status))}</span><h2 class="section-gap">${esc(j.address||j.id)}</h2><p>${esc(j.id)} · ${esc(j.requestId||'')}</p><div class="detail-block section-gap"><strong>${esc(j.worker||t('Unassigned','Sin asignar'))}</strong><p>${j.checkInAt?t('Checked in · Realtor can see this arrival.','Ya llegó · el Realtor ve esta llegada.'):t('Waiting for crew update.','Esperando actualización del equipo.')}</p></div>${crew.length?`<div class="assign-row"><select class="inline-select" data-assign-select="${esc(j.id)}" aria-label="${t('Worker','Trabajador')}">${crew.map(w=>`<option value="${w.id}"${j.workerId===w.id?' selected':''}>${esc(w.name)}</option>`).join('')}</select><button class="button primary small" data-assign-wo="${esc(j.id)}" data-assign-req="${esc(j.requestId||'')}">${j.workerId?t('Reassign','Reasignar'):t('Assign','Asignar')}</button></div>`:`<p class="muted-note">${t('Workers appear here when they create a profile in the Field Team portal.','Los trabajadores aparecen aquí cuando crean su perfil en el portal del equipo.')}</p>`}</article>`).join(''):`<article class="card"><p>${t('When a Realtor request becomes a work order, it appears here to assign it.','Cuando una solicitud del Realtor se vuelve orden de trabajo, aparece aquí para asignarla.')}</p></article>`;
  return `${pageHead('FIELD HANDOFF','Your field team, their requests and assignments','Tu equipo de campo, sus solicitudes y asignaciones','Requests sent from the Field Team portal arrive here. Send them to the intake queue or decline them.','Las solicitudes enviadas desde el portal del equipo llegan aquí. Pásalas a la cola de solicitudes o recházalas.',`<button class="button secondary" data-add-contact="Worker">+ ${t('Add field worker','Agregar trabajador')}</button><a class="button secondary" href="${PORTALS.worker}" target="_blank" rel="noopener">${t('Open worker portal','Abrir portal trabajador')} ↗</a>`)}
  <section class="card table-card"><div class="table-head"><div><span class="eyebrow">${t('FROM THE FIELD TEAM','DEL EQUIPO DE CAMPO')}</span><h2>${t('Property and estimate requests','Solicitudes de propiedades y estimados')}</h2></div></div><table class="table"><thead><tr><th>${t('Worker','Trabajador')}</th><th>${t('Property','Propiedad')}</th><th>${t('Services and final price','Servicios y valor final')}</th><th>${t('Total','Total')}</th><th>${t('Status','Estado')}</th><th></th></tr></thead><tbody>${reqRows}</tbody></table></section>
  <h2 class="section-title">${t('Team','Equipo')}</h2><div class="grid cols-3">${team}</div>
  <h2 class="section-title">${t('Work orders','Órdenes de trabajo')}</h2><div class="grid cols-3">${orders}</div>`;
}
const estimateStatusText={Draft:['Draft','Borrador'],Sent:['Sent to client','Enviado al cliente'],Approved:['Approved','Aprobado'],Declined:['Declined','Rechazado']};
function estimateStatusTag(s){const cls={Draft:'',Sent:'aqua',Approved:'green',Declined:'red'}[s]??'';return `<span class="tag ${cls}">${t(...(estimateStatusText[s]||[s,s]))}</span>`}
function liveEstimates(){
  const hub=(state.hub?.estimates||[]).map(e=>({...e,address:e.address||state.requests.find(x=>x.dbId===e.requestId)?.address||''}));
  return [...(state.localEstimates||[]),...hub];
}
function estimatesView(){
  const list=liveEstimates();
  const body=list.length?list.map(e=>{
    const next=e.source==='local'?(e.status==='Draft'?`<button class="button secondary small" data-estimate-status="${esc(e.publicId)}" data-status="Sent">${t('Send to client','Enviar al cliente')}</button>`:e.status==='Sent'?`<button class="button primary small" data-estimate-status="${esc(e.publicId)}" data-status="Approved">${t('Mark approved','Marcar aprobado')}</button><button class="link-button" data-estimate-status="${esc(e.publicId)}" data-status="Declined">${t('Declined','Rechazado')}</button>`:''):'';
    return `<tr class="${e.publicId&&e.publicId===state.justAddedEstimate?'just-created':''}"><td><strong>${esc(e.publicId||e.id)}</strong>${e.requestId&&e.source==='local'?`<small class="person-sub">${esc(e.requestId)}</small>`:''}</td><td><strong>${esc(e.address||'—')}</strong>${e.client?`<small class="person-sub">${esc(e.client)}</small>`:''}</td><td>${(e.lines||[]).length||'—'}</td><td><strong>${money(e.total)}</strong></td><td>${estimateStatusTag(e.status||'Draft')}</td><td><div class="row-actions">${e.source==='local'?`<button class="link-button" data-estimate-view="${esc(e.publicId)}">${t('View','Ver')}</button>`:''}${next}</div></td></tr>`;
  }).join(''):`<tr><td colspan="6">${t('No estimates yet. Use “New estimate” or pick a request below.','Aún no hay estimados. Usa “Nuevo estimado” o elige una solicitud abajo.')}</td></tr>`;
  const withEstimate=new Set(list.map(e=>e.requestId).filter(Boolean));
  const pending=state.requests.filter(r=>!withEstimate.has(r.id)&&!withEstimate.has(r.dbId));
  const pendingHtml=pending.length?pending.slice(0,12).map(r=>`<div class="connection-row"><div><strong>${esc(r.address)}</strong><small>${esc(r.id)} · ${esc(r.contact||'')} · ${statusText(r.status)}</small></div><button class="button secondary small" data-estimate-for="${esc(r.id)}">+ ${t('Create estimate','Crear estimado')}</button></div>`).join(''):`<p class="muted-note">${t('Every request already has an estimate.','Todas las solicitudes ya tienen estimado.')}</p>`;
  return `${pageHead('ESTIMATES','Build, approve and send client-ready options','Crea, aprueba y envía opciones','Internal costs stay in Management. The Realtor and client receive only approved scope, ranges and terms.','Los costos internos quedan en Management. El Realtor y cliente reciben alcance y precios aprobados.',`<button class="button primary" data-action="new-estimate">+ ${t('New estimate','Nuevo estimado')}</button>`)}
  <section class="card table-card"><table class="table"><thead><tr><th>${t('Estimate','Estimado')}</th><th>${t('Property','Propiedad')}</th><th>${t('Services','Servicios')}</th><th>${t('Total','Total')}</th><th>${t('Status','Estado')}</th><th></th></tr></thead><tbody>${body}</tbody></table></section>
  <section class="card section-gap"><div class="card-head"><div><span class="eyebrow">${t('READY TO PRICE','LISTAS PARA ESTIMAR')}</span><h2>${t('Requests without an estimate','Solicitudes sin estimado')}</h2></div></div><div class="connection-list">${pendingHtml}</div></section>`;
}
function estimateLineRow(l,i){
  const c=state.catalog.find(x=>x.id===l.serviceId);
  return `<tr><td><strong>${esc(l.service)}</strong>${c?`<small class="person-sub">${t('Catalog','Catálogo')}: ${money(c.low)}–${money(c.high)} / ${esc(c.unit)}</small>`:''}</td><td><input type="number" min="0.5" step="0.5" value="${l.qty}" data-line-qty="${i}"></td><td><input type="number" min="0" step="1" value="${l.price}" data-line-price="${i}"></td><td><strong>${money(l.qty*l.price)}</strong></td><td><button type="button" class="close" data-line-remove="${i}" aria-label="${t('Remove','Quitar')}">×</button></td></tr>`;
}
function paintEstimateLines(){
  const lines=state.estimateDraft;
  el('estLines').innerHTML=lines.length?lines.map(estimateLineRow).join(''):`<tr><td colspan="5" class="muted-note">${t('Search the catalog above to add services.','Busca en el catálogo para agregar servicios.')}</td></tr>`;
  el('estTotal').textContent=money(lines.reduce((s,l)=>s+l.qty*l.price,0));
  el('estLines').querySelectorAll('[data-line-qty]').forEach(i=>i.oninput=()=>{lines[+i.dataset.lineQty].qty=Math.max(0,+i.value||0);el('estTotal').textContent=money(lines.reduce((s,l)=>s+l.qty*l.price,0));i.closest('tr').children[3].innerHTML=`<strong>${money(lines[+i.dataset.lineQty].qty*lines[+i.dataset.lineQty].price)}</strong>`});
  el('estLines').querySelectorAll('[data-line-price]').forEach(i=>i.oninput=()=>{lines[+i.dataset.linePrice].price=Math.max(0,+i.value||0);el('estTotal').textContent=money(lines.reduce((s,l)=>s+l.qty*l.price,0));i.closest('tr').children[3].innerHTML=`<strong>${money(lines[+i.dataset.linePrice].qty*lines[+i.dataset.linePrice].price)}</strong>`});
  el('estLines').querySelectorAll('[data-line-remove]').forEach(b=>b.onclick=()=>{lines.splice(+b.dataset.lineRemove,1);paintEstimateLines()});
}
function openEstimateModal(requestId=''){
  const m=ensureContactModal();state.estimateDraft=[];
  m.innerHTML=`<form class="modal-card estimate-card" id="estimateForm" novalidate><div class="modal-head"><div><span class="eyebrow">ESTIMATES</span><h2>${t('New estimate','Nuevo estimado')}</h2></div><button type="button" class="close" data-contact-close>×</button></div>
  <div class="form-grid"><label class="full"><span>${t('Request','Solicitud')}</span><select id="estRequest"><option value="">${t('No linked request','Sin solicitud vinculada')}</option>${state.requests.map(r=>`<option value="${esc(r.id)}">${esc(r.id)} · ${esc(r.address)}</option>`).join('')}</select></label><label><span>${t('Property address','Dirección de la propiedad')} *</span><input id="estAddress" placeholder="${t('Street, city, state','Calle, ciudad, estado')}"></label><label><span>${t('Client','Cliente')} *</span><input id="estClient" placeholder="${t('Client or Realtor','Cliente o Realtor')}"></label>
  <label class="full"><span>${t('Add services from the price catalog','Agrega servicios del catálogo de precios')}</span><input id="estSearch" autocomplete="off" placeholder="${t('Search: light fixture, disposal, roof…','Busca: lámpara, disposal, techo…')}"><div class="est-results" id="estResults"></div></label></div>
  <table class="table est-lines"><thead><tr><th>${t('Service','Servicio')}</th><th>${t('Qty','Cant.')}</th><th>${t('Price','Precio')}</th><th>${t('Subtotal','Subtotal')}</th><th></th></tr></thead><tbody id="estLines"></tbody><tfoot><tr><td colspan="3"><strong>${t('Total','Total')}</strong></td><td colspan="2"><strong id="estTotal">$0</strong></td></tr></tfoot></table>
  <div class="form-grid"><label class="full"><span>${t('Notes for the client','Notas para el cliente')}</span><textarea id="estNotes" rows="2"></textarea></label></div>
  <div class="form-warning" id="estError" hidden></div>
  <div class="modal-actions"><button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit" id="estSave">${t('Save estimate','Guardar estimado')}</button></div></form>`;
  m.querySelectorAll('[data-contact-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
  const pick=id=>{const r=state.requests.find(x=>x.id===id);if(r){el('estAddress').value=r.address||'';el('estClient').value=r.client||r.contact||''}};
  el('estRequest').onchange=e=>pick(e.target.value);
  el('estSearch').oninput=e=>{
    const q=e.target.value.trim().toLowerCase(),box=el('estResults');
    if(q.length<2){box.innerHTML='';return}
    const hits=state.catalog.filter(r=>[r.service,r.category,r.customer_en,r.customer_es].join(' ').toLowerCase().includes(q)).slice(0,8);
    box.innerHTML=hits.length?hits.map(r=>`<button type="button" data-add-service="${esc(r.id)}"><strong>${esc(r.service)}</strong><small>${esc(r.category)} · ${money(r.low)}–${money(r.high)} / ${esc(r.unit)}</small></button>`).join(''):`<small class="muted-note">${t('No matching service','Sin servicios con ese nombre')}</small>`;
    box.querySelectorAll('[data-add-service]').forEach(b=>b.onclick=()=>{const c=state.catalog.find(x=>x.id===b.dataset.addService);state.estimateDraft.push({serviceId:c.id,service:c.service,qty:1,unit:c.unit,price:c.high});el('estSearch').value='';box.innerHTML='';paintEstimateLines();el('estSearch').focus()});
  };
  el('estimateForm').onsubmit=e=>{e.preventDefault();saveEstimate()};
  if(requestId){el('estRequest').value=requestId;pick(requestId)}
  paintEstimateLines();
  m.classList.add('open');setTimeout(()=>(requestId?el('estSearch'):el('estAddress')).focus(),50);
}
async function saveEstimate(){
  const err=el('estError'),btn=el('estSave'),show=msg=>{err.textContent='⚠ '+msg;err.hidden=false};
  const lines=state.estimateDraft;
  const payload={requestId:el('estRequest').value||null,address:el('estAddress').value.trim(),client:el('estClient').value.trim(),lines,notes:el('estNotes').value.trim(),createdBy:state.account?.name||''};
  err.hidden=true;
  if(payload.address.length<5){show(t('Write the property address.','Escribe la dirección de la propiedad.'));el('estAddress').focus();return}
  if(payload.client.length<2){show(t('Write the client name.','Escribe el nombre del cliente.'));el('estClient').focus();return}
  if(!lines.length){show(t('Add at least one service from the catalog.','Agrega al menos un servicio del catálogo.'));el('estSearch').focus();return}
  if(lines.some(l=>!(l.qty>0))){show(t('Every service needs a quantity above 0.','Cada servicio necesita una cantidad mayor a 0.'));return}
  btn.disabled=true;btn.textContent=t('Saving…','Guardando…');
  try{
    const res=await fetch('/api/operations/estimates',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});
    const body=await res.json().catch(()=>({}));
    if(!res.ok){show(apiError(body)||t('Could not save. Try again.','No se pudo guardar. Intenta de nuevo.'));return}
    state.localEstimates=[body,...(state.localEstimates||[])];state.justAddedEstimate=body.publicId;
    el('contactModal').classList.remove('open');render();
    toast(`✓ ${t('Estimate saved','Estimado guardado')}: ${body.publicId} · ${money(body.total)}`,5000);
  }catch{show(t('Could not save. Check your connection and try again.','No se pudo guardar. Revisa tu conexión e intenta de nuevo.'))}
  finally{btn.disabled=false;btn.textContent=t('Save estimate','Guardar estimado')}
}
function viewEstimate(publicId){
  const e=(state.localEstimates||[]).find(x=>x.publicId===publicId);if(!e)return;
  const m=ensureContactModal();
  m.innerHTML=`<div class="modal-card estimate-card"><div class="modal-head"><div><span class="eyebrow">${esc(e.publicId)}${e.requestId?' · '+esc(e.requestId):''}</span><h2>${esc(e.address)}</h2></div><button type="button" class="close" data-contact-close>×</button></div>
  <p class="muted-note">${esc(e.client)} · ${estimateStatusTag(e.status)}</p>
  <table class="table est-lines"><thead><tr><th>${t('Service','Servicio')}</th><th>${t('Qty','Cant.')}</th><th>${t('Price','Precio')}</th><th>${t('Subtotal','Subtotal')}</th></tr></thead><tbody>${(e.lines||[]).map(l=>`<tr><td>${esc(l.service)}</td><td>${l.qty}</td><td>${money(l.price)}</td><td><strong>${money(l.qty*l.price)}</strong></td></tr>`).join('')}</tbody><tfoot><tr><td colspan="3"><strong>${t('Total','Total')}</strong></td><td><strong>${money(e.total)}</strong></td></tr></tfoot></table>
  ${e.notes?`<p class="muted-note">${esc(e.notes)}</p>`:''}<div class="modal-actions"><button type="button" class="button secondary" data-contact-close>${t('Close','Cerrar')}</button></div></div>`;
  m.querySelectorAll('[data-contact-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
  m.classList.add('open');
}
async function setEstimateStatus(btn,publicId,status){
  btn.disabled=true;
  try{
    const res=await fetch('/api/operations/estimates/'+encodeURIComponent(publicId),{method:'PATCH',headers:{'Content-Type':'application/json'},body:JSON.stringify({status})});
    if(!res.ok)throw new Error();
    const saved=await res.json();
    state.localEstimates=(state.localEstimates||[]).map(x=>x.publicId===publicId?saved:x);state.justAddedEstimate=null;
    const req=(state.localRequests||[]).find(r=>r.id===saved.requestId);
    if(req&&status==='Approved'){
      const r2=await fetch('/api/operations/requests/'+encodeURIComponent(req.id),{method:'PATCH',headers:{'Content-Type':'application/json'},body:JSON.stringify({value:saved.total,items:(saved.lines||[]).length})});
      if(r2.ok){const fresh=await r2.json();state.localRequests=state.localRequests.map(r=>r.id===req.id?fresh:r);mergeRequests()}
    }
    render();toast(`${saved.publicId}: ${t(...estimateStatusText[status])}`);
  }catch{btn.disabled=false;toast(t('Could not update the estimate. Try again.','No se pudo actualizar el estimado. Intenta de nuevo.'))}
}
const invoiceStatusText={Open:['Open','Abierta','gold'],Paid:['Paid','Pagada','green'],Void:['Void','Anulada','red']};
function invoiceStatusTag(s){const x=invoiceStatusText[s]||[s,s,''];return `<span class="tag ${x[2]}">${esc(t(x[0],x[1]))}</span>`}
function allInvoices(){
  const hub=(state.hub?.invoices||[]).map(i=>({publicId:i.publicId||String(i.id),address:state.requests.find(r=>r.dbId===i.requestId)?.address||'—',client:'',total:i.total,status:/paid/i.test(i.status||'')?'Paid':'Open',dueDate:'',source:'hub'}));
  return [...(state.localInvoices||[]),...hub];
}
function invoicesView(){
  const list=allInvoices(),open=list.filter(i=>i.status==='Open');
  const paid=list.filter(i=>i.status==='Paid').reduce((s,i)=>s+(Number(i.total)||0),0);
  const body=list.length?list.map(i=>`<tr class="${i.publicId===state.justAddedInvoice?'just-created':''}"><td><strong>${esc(i.publicId)}</strong>${i.estimateId?`<small class="person-sub">${esc(i.estimateId)}</small>`:''}</td><td><strong>${esc(i.address)}</strong>${i.client?`<small class="person-sub">${esc(i.client)}</small>`:''}</td><td>${esc(i.dueDate||'—')}</td><td><strong>${money(i.total)}</strong></td><td>${invoiceStatusTag(i.status)}</td><td><div class="row-actions">${i.source==='local'&&i.status==='Open'?`<button class="button primary small" data-invoice-status="${esc(i.publicId)}" data-status="Paid">${t('Record payment','Registrar pago')}</button><button class="link-button" data-invoice-status="${esc(i.publicId)}" data-status="Void">${t('Void','Anular')}</button>`:''}</div></td></tr>`).join(''):`<tr><td colspan="6">${t('No invoices yet. Use “Create invoice” or bill an approved project.','Aún no hay invoices. Usa “Crear invoice” o factura un proyecto aprobado.')}</td></tr>`;
  return `${pageHead('FINANCIAL CONTROL','Invoices, deposits and payments','Invoices, depósitos y pagos','Generate invoices from approved estimates and keep payment status tied to the property and project.','Genera invoices desde estimados aprobados y guarda el pago con la propiedad.',`<button class="button primary" data-action="new-invoice">+ ${t('Create invoice','Crear invoice')}</button>`)}<div class="grid cols-3">${metric(t('PAID','PAGADO'),money(paid),t('Recorded payments','Pagos registrados'))}${metric(t('OPEN RECEIVABLES','POR COBRAR'),money(open.reduce((s,i)=>s+(Number(i.total)||0),0)),t(`${open.length} open invoice(s)`,`${open.length} invoice(s) abierta(s)`),open.length>0)}${metric(t('APPROVED TO BILL','APROBADOS POR FACTURAR'),String((state.localEstimates||[]).filter(e=>e.status==='Approved'&&!invoiceFor(e.publicId)).length),t('Approved estimates without invoice','Estimados aprobados sin invoice'))}</div><section class="card table-card section-gap"><table class="table"><thead><tr><th>Invoice</th><th>${t('Property','Propiedad')}</th><th>${t('Due','Vence')}</th><th>${t('Total','Total')}</th><th>${t('Status','Estado')}</th><th></th></tr></thead><tbody>${body}</tbody></table></section>`;
}
function openInvoiceModal(estimateId=''){
  const approved=(state.localEstimates||[]).filter(e=>e.status==='Approved'&&!invoiceFor(e.publicId));
  const {form,show}=modalShell('invoiceForm','FINANCIAL CONTROL',t('Create invoice','Crear invoice'),`<div class="form-grid"><label class="full"><span>${t('Approved estimate','Estimado aprobado')}</span><select id="invEstimate"><option value="">${t('No estimate (manual invoice)','Sin estimado (invoice manual)')}</option>${approved.map(e=>`<option value="${esc(e.publicId)}">${esc(e.publicId)} · ${esc(e.address)} · ${money(e.total)}</option>`).join('')}</select></label><label><span>${t('Property address','Dirección de la propiedad')} *</span><input id="invAddress"></label><label><span>${t('Client','Cliente')} *</span><input id="invClient"></label><label><span>${t('Total (USD)','Total (USD)')} *</span><input id="invTotal" type="number" min="1" step="0.01"></label><label><span>${t('Due date','Fecha de vencimiento')}</span><input id="invDue" type="date" value="${isoDate(15)}" min="${isoDate(0)}"></label><label class="full"><span>${t('Notes','Notas')}</span><textarea id="invNotes" rows="2"></textarea></label></div>`,`<button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit" id="invSave">${t('Create invoice','Crear invoice')}</button>`);
  const pick=id=>{const e=approved.find(x=>x.publicId===id);if(e){el('invAddress').value=e.address;el('invClient').value=e.client;el('invTotal').value=e.total}};
  el('invEstimate').onchange=e=>pick(e.target.value);
  if(estimateId){el('invEstimate').value=estimateId;pick(estimateId)}
  setTimeout(()=>(estimateId?el('invDue'):el('invEstimate')).focus(),50);
  form.onsubmit=async e=>{
    e.preventDefault();
    const payload={estimateId:el('invEstimate').value,address:el('invAddress').value.trim(),client:el('invClient').value.trim(),total:Number(el('invTotal').value),dueDate:el('invDue').value||null,notes:el('invNotes').value.trim(),createdBy:state.account?.name||''};
    if(payload.address.length<5){show(t('Write the property address.','Escribe la dirección de la propiedad.'));el('invAddress').focus();return}
    if(payload.client.length<2){show(t('Write the client name.','Escribe el nombre del cliente.'));el('invClient').focus();return}
    if(!(payload.total>0)){show(t('The total must be above $0.','El total debe ser mayor a $0.'));el('invTotal').focus();return}
    const done=busyButton(el('invSave'));
    try{
      const saved=await postJson('/api/operations/invoices',payload);
      state.localInvoices=[saved,...state.localInvoices];state.justAddedInvoice=saved.publicId;
      el('contactModal').classList.remove('open');navigate('invoices');
      toast(`✓ ${t('Invoice created','Invoice creada')}: ${saved.publicId} · ${money(saved.total)}`,5000);
    }catch(err){show(err.status===409?t('This estimate already has an invoice.','Este estimado ya tiene invoice.'):err.message)}
    finally{done()}
  };
}
async function setInvoiceStatus(btn,publicId,status){
  if(status==='Void'&&!confirm(t('Void this invoice?','¿Anular esta invoice?')))return;
  btn.disabled=true;
  try{const saved=await postJson('/api/operations/invoices/'+encodeURIComponent(publicId),{status},'PATCH');state.localInvoices=state.localInvoices.map(i=>i.publicId===publicId?saved:i);render();toast(`${saved.publicId}: ${t(...invoiceStatusText[status].slice(0,2))}`)}
  catch(err){btn.disabled=false;toast(err.message)}
}
function catalogView(){const q=(state.catalogQuery||'').toLowerCase();const all=state.catalog.filter(r=>!q||[r.id,r.service,r.category,r.customer_en,r.customer_es,r.inspector].join(' ').toLowerCase().includes(q)).sort((a,b)=>String(b.updatedAt||'').localeCompare(String(a.updatedAt||'')));const rows=all.slice(0,60);return `${pageHead('SHARED PRICING','The price source used by every portal','La fuente de precios para todos los portales','Management controls the approved range. Realtor and worker portals read the same service ID and version.','Management controla el rango aprobado. Los otros portales leen el mismo ID y versión.',`<button class="button primary" data-action="new-repair">+ ${t('Add repair item','Agregar reparación')}</button>`)}<div class="catalog-toolbar"><div class="search-box"><span>⌕</span><input id="catalogSearch" value="${esc(state.catalogQuery||'')}" placeholder="${t('Search repair, inspector wording or category','Buscar reparación, palabra del inspector o categoría')}"></div><button class="button secondary" data-action="export-catalog">${t('Export catalog','Exportar catálogo')}</button></div>${all.length>rows.length?`<p class="muted-note">${t(`Showing ${rows.length} of ${all.length}. Search to find the rest.`,`Mostrando ${rows.length} de ${all.length}. Busca para encontrar el resto.`)}</p>`:''}<div class="catalog-grid">${rows.length?'':`<p class="muted-note">${t('No repair matches that search.','Ninguna reparación coincide con esa búsqueda.')}</p>`}${rows.map(r=>`<article class="catalog-item${r.id===state.justSavedRepair?' just-created':''}"><span class="tag aqua">${esc(r.category)}</span>${r.origin?`<span class="tag ${r.origin==='custom'?'green':'gold'} catalog-origin">${r.origin==='custom'?t('New','Nueva'):t('Edited','Editada')}</span>`:''}<h3>${esc(r.service)}</h3><div class="range">${Number.isFinite(Number(r.low))?(Number(r.low)===Number(r.high)?money(r.low):`${money(r.low)}–${money(r.high)}`):t('Management review','Revisión')}</div><p>${esc(r.inspector||r.notes)}</p><footer><span class="tag ${String(r.automatic).toLowerCase()==='yes'?'green':'gold'}">${String(r.automatic).toLowerCase()==='yes'?t('Instant range','Rango inmediato'):t('Review','Revisar')}</span><button class="button secondary small" data-edit-repair="${esc(r.id)}">${t('Edit','Editar')}</button></footer></article>`).join('')}</div>`}
function openRepairModal(id=null){
  const r=id?state.catalog.find(x=>x.id===id):null;if(id&&!r)return;
  const cats=[...new Set(state.catalog.map(x=>x.category))].sort();
  const v=k=>esc(r?.[k]??'');
  const m=ensureContactModal();
  m.innerHTML=`<form class="modal-card estimate-card" id="repairForm" novalidate><div class="modal-head"><div><span class="eyebrow">SHARED PRICING${r?' · '+esc(r.id):''}</span><h2>${r?t('Edit repair','Editar reparación'):t('Add repair item','Agregar reparación')}</h2></div><button type="button" class="close" data-contact-close>×</button></div>
  <div class="form-grid"><label><span>${t('Repair name','Nombre de la reparación')} *</span><input id="rpService" value="${v('service')}" placeholder="${t('Standard light fixture swap','Cambio de lámpara estándar')}"></label><label><span>${t('Category','Categoría')} *</span><input id="rpCategory" list="rpCategories" value="${v('category')}" placeholder="${t('Pick or write a new one','Elige o escribe una nueva')}"><datalist id="rpCategories">${cats.map(c=>`<option value="${esc(c)}">`).join('')}</datalist></label>
  <label><span>${t('Low price','Precio mínimo')} (USD) *</span><input id="rpLow" type="number" min="0" step="1" value="${r?.low??''}"></label><label><span>${t('High price','Precio máximo')} (USD) *</span><input id="rpHigh" type="number" min="0" step="1" value="${r?.high??''}"></label>
  <label><span>${t('Unit','Unidad')} *</span><input id="rpUnit" value="${v('unit')||'each'}" placeholder="each, sq ft, linear ft"></label><label><span>${t('Pricing','Precio')}</span><select id="rpAutomatic">${[['Yes','Instant range','Rango inmediato'],['Conditional','Instant, with conditions','Inmediato con condiciones'],['No','Management review','Revisión de Management']].map(o=>`<option value="${o[0]}"${(r?.automatic||'Yes')===o[0]?' selected':''}>${t(o[1],o[2])}</option>`).join('')}</select></label>
  <label><span>${t('Technicians','Técnicos')}</span><input id="rpTechs" type="number" min="0" step="1" value="${r?.techs??1}"></label><label><span>${t('Estimated hours','Horas estimadas')}</span><input id="rpHours" type="number" min="0" step="0.5" value="${r?.hours??1}"></label>
  <label class="full"><span>${t('Inspector wording','Palabras del inspector')}</span><input id="rpInspector" value="${v('inspector')}" placeholder="${t('light fixture inoperable; luminaire replacement','lámpara no funciona; reemplazo de luminaria')}"></label>
  <label><span>${t('How clients say it (EN)','Cómo lo dice el cliente (EN)')}</span><input id="rpCustomerEn" value="${v('customer_en')}"></label><label><span>${t('How clients say it (ES)','Cómo lo dice el cliente (ES)')}</span><input id="rpCustomerEs" value="${v('customer_es')}"></label>
  <label><span>${t('Min technician-hours per job','Horas-técnico mínimas por trabajo')}</span><input id="rpHoursLow" type="number" min="0" step="0.5" value="${r?.hours_low??''}" placeholder="${t('Optional','Opcional')}"></label><label><span>${t('Max technician-hours per job','Horas-técnico máximas por trabajo')}</span><input id="rpHoursHigh" type="number" min="0" step="0.5" value="${r?.hours_high??''}" placeholder="${t('Optional','Opcional')}"></label>
  <label><span>${t('Materials cost, min per unit','Costo de materiales mínimo por unidad')} (USD)</span><input id="rpMatLow" type="number" min="0" step="1" value="${r?.materials_low??''}" placeholder="${t('Optional','Opcional')}"></label><label><span>${t('Materials cost, max per unit','Costo de materiales máximo por unidad')} (USD)</span><input id="rpMatHigh" type="number" min="0" step="1" value="${r?.materials_high??''}" placeholder="${t('Optional','Opcional')}"></label>
  <label><span>${t('Materials','Materiales')}</span><input id="rpMaterials" value="${v('materials')}"></label><label><span>${t('Needs review when','Requiere revisión si')}</span><input id="rpReview" value="${v('review')}"></label>
  <label class="full"><span>${t('Notes','Notas')}</span><textarea id="rpNotes" rows="2">${v('notes')}</textarea></label></div>
  <div class="form-warning" id="rpError" hidden></div>
  <div class="modal-actions"><button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit" id="rpSave">${r?t('Save changes','Guardar cambios'):t('Add to catalog','Agregar al catálogo')}</button></div></form>`;
  m.querySelectorAll('[data-contact-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
  el('repairForm').onsubmit=e=>{e.preventDefault();saveRepair(r)};
  m.classList.add('open');setTimeout(()=>el('rpService').focus(),50);
}
async function saveRepair(existing){
  const err=el('rpError'),btn=el('rpSave'),show=msg=>{err.textContent='⚠ '+msg;err.hidden=false};
  const num=id=>el(id).value===''?NaN:Number(el(id).value);
  const opt=id=>el(id).value===''?null:Number(el(id).value);
  const payload={...(existing||{}),service:el('rpService').value.trim(),category:el('rpCategory').value.trim(),low:num('rpLow'),high:num('rpHigh'),unit:el('rpUnit').value.trim(),automatic:el('rpAutomatic').value,techs:num('rpTechs')||0,hours:num('rpHours')||0,hours_low:opt('rpHoursLow'),hours_high:opt('rpHoursHigh'),materials_low:opt('rpMatLow'),materials_high:opt('rpMatHigh'),inspector:el('rpInspector').value.trim(),customer_en:el('rpCustomerEn').value.trim(),customer_es:el('rpCustomerEs').value.trim(),materials:el('rpMaterials').value.trim(),review:el('rpReview').value.trim(),notes:el('rpNotes').value.trim(),updatedBy:state.account?.name||''};
  err.hidden=true;
  if(payload.service.length<3){show(t('Write the repair name.','Escribe el nombre de la reparación.'));el('rpService').focus();return}
  if(!payload.category){show(t('Choose a category.','Elige una categoría.'));el('rpCategory').focus();return}
  if(!(payload.low>=0)||!(payload.high>=0)){show(t('Write the low and high prices.','Escribe el precio mínimo y el máximo.'));el(Number.isNaN(payload.low)?'rpLow':'rpHigh').focus();return}
  if(payload.high<payload.low){show(t('The high price must be equal to or above the low price.','El precio máximo debe ser igual o mayor que el mínimo.'));el('rpHigh').focus();return}
  if(!payload.unit){show(t('Write the unit, like each or sq ft.','Escribe la unidad, como each o sq ft.'));el('rpUnit').focus();return}
  if([payload.hours_low,payload.hours_high,payload.materials_low,payload.materials_high].some(x=>x!=null&&!(x>=0))){show(t('Hours and material costs cannot be negative.','Las horas y los costos de materiales no pueden ser negativos.'));return}
  if(payload.hours_low!=null&&payload.hours_high!=null&&payload.hours_high<payload.hours_low){show(t('The maximum hours must be equal to or above the minimum.','Las horas máximas deben ser iguales o mayores que las mínimas.'));el('rpHoursHigh').focus();return}
  if(payload.materials_low!=null&&payload.materials_high!=null&&payload.materials_high<payload.materials_low){show(t('The maximum material cost must be equal to or above the minimum.','El costo máximo de materiales debe ser igual o mayor que el mínimo.'));el('rpMatHigh').focus();return}
  btn.disabled=true;btn.textContent=t('Saving…','Guardando…');
  try{
    const res=await fetch(existing?'/api/operations/catalog/'+encodeURIComponent(existing.id):'/api/operations/catalog',{method:existing?'PUT':'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});
    const body=await res.json().catch(()=>({}));
    if(res.status===409){show(t(`“${body.existing?.service}” already exists (${body.existing?.id}).`,`“${body.existing?.service}” ya existe (${body.existing?.id}).`));return}
    if(!res.ok){show(apiError(body)||t('Could not save. Try again.','No se pudo guardar. Intenta de nuevo.'));return}
    state.catalog=existing?state.catalog.map(x=>x.id===body.id?body:x):[...state.catalog,body];
    state.justSavedRepair=body.id;el('catalogCount').textContent=state.catalog.length;
    el('contactModal').classList.remove('open');render();
    toast(`✓ ${existing?t('Repair updated','Reparación actualizada'):t('Repair added','Reparación agregada')}: ${body.service} (${body.id})`,5000);
  }catch{show(t('Could not save. Check your connection and try again.','No se pudo guardar. Revisa tu conexión e intenta de nuevo.'))}
  finally{btn.disabled=false;btn.textContent=existing?t('Save changes','Guardar cambios'):t('Add to catalog','Agregar al catálogo')}
}
function exportCatalog(){
  const cols=['id','category','service','low','high','unit','automatic','tier','pricing_model','techs','hours','inspector','customer_en','customer_es','materials','review','notes'];
  const cell=v=>{const s=String(v??'');return /[",\n]/.test(s)?`"${s.replace(/"/g,'""')}"`:s};
  const csv='\ufeff'+[cols.join(','),...state.catalog.map(r=>cols.map(c=>cell(r[c])).join(','))].join('\r\n');
  const a=document.createElement('a');a.href=URL.createObjectURL(new Blob([csv],{type:'text/csv;charset=utf-8'}));a.download=`safe-price-catalog-${isoDate(0)}.csv`;a.click();setTimeout(()=>URL.revokeObjectURL(a.href),1000);
  toast(t(`Catalog exported: ${state.catalog.length} repairs`,`Catálogo exportado: ${state.catalog.length} reparaciones`));
}
function promotionsView(){const list=livePromos();const cards=list.length?list.map(p=>`<article class="card"><span class="tag ${/live/i.test(p.status)?'green':'gold'}">${esc((p.status||'Draft').toUpperCase())}</span><h2 class="section-gap">${esc(p.title)}</h2><p>${esc(p.offer||'')}</p><div class="detail-block section-gap"><strong>${esc(p.publicId||'')}</strong><p>${esc(p.body||'')}</p></div></article>`).join(''):`<article class="card"><p>${t('No promotions in IndorDB yet.','Aún no hay promociones en IndorDB.')}</p></article>`;return `${pageHead('OFFERS & CONTENT','Control what Realtors can share','Controla lo que pueden compartir','Create, approve, schedule or retire service promotions without changing the Realtor portal code.','Crea, aprueba, programa o retira promociones sin cambiar el código.',`<button class="button primary" data-action="new-promo">+ ${t('Create promotion','Crear promoción')}</button>`)}<div class="grid cols-3">${cards}</div>`}
function openPromoModal(){
  const {form,show}=modalShell('promoForm','OFFERS & CONTENT',t('Create promotion','Crear promoción'),`<div class="form-grid"><label class="full"><span>${t('Title','Título')} *</span><input id="prTitle" maxlength="160"></label><label><span>${t('Offer','Oferta')}</span><input id="prOffer" maxlength="200" placeholder="${t('e.g. 10% off gutter cleaning','Ej.: 10% de descuento en limpieza de canaletas')}"></label><label><span>${t('Status','Estado')}</span><select id="prStatus"><option value="Draft">${t('Draft','Borrador')}</option><option value="Live">${t('Live','Activa')}</option></select></label><label class="full"><span>${t('Description for Realtors','Descripción para los Realtors')}</span><textarea id="prBody" rows="3"></textarea></label></div>`,`<button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit" id="prSave">${t('Save promotion','Guardar promoción')}</button>`);
  setTimeout(()=>el('prTitle').focus(),50);
  form.onsubmit=async e=>{
    e.preventDefault();
    const payload={title:el('prTitle').value.trim(),offer:el('prOffer').value.trim(),body:el('prBody').value.trim(),status:el('prStatus').value};
    if(payload.title.length<3){show(t('Write the promotion title.','Escribe el título de la promoción.'));el('prTitle').focus();return}
    const done=busyButton(el('prSave'));
    try{await postJson('/api/safe/promotions',payload);await refreshHub(false);el('contactModal').classList.remove('open');render();toast(`✓ ${t('Promotion saved','Promoción guardada')}: ${payload.title}`,5000)}
    catch(err){show(err.message)}
    finally{done()}
  };
}
function giftCode(){const abc='ABCDEFGHJKLMNPQRSTUVWXYZ23456789';return 'SAFE-'+Array.from(crypto.getRandomValues(new Uint8Array(6)),n=>abc[n%abc.length]).join('')}
function openGiftModal(){
  const firms=[...new Set(liveContacts().filter(c=>c.role==='Realtor').map(c=>c.firm||c.name).filter(Boolean))];
  const {form,show}=modalShell('giftForm','PARTNER PROGRAM',t('Create gift card','Crear gift card'),`<div class="form-grid"><label><span>${t('Client name','Nombre del cliente')} *</span><input id="gcClient" maxlength="160"></label><label><span>${t('Property address','Dirección de la propiedad')}</span><input id="gcAddress" maxlength="200"></label><label><span>${t('Realtor or firm','Realtor o firma')}</span><input id="gcFirm" list="gcFirms" maxlength="160"><datalist id="gcFirms">${firms.map(f=>`<option value="${esc(f)}">`).join('')}</datalist></label><label><span>${t('Amount (USD)','Monto (USD)')} *</span><select id="gcAmount">${[50,100,150,200,250].map(a=>`<option value="${a}">${money(a)}</option>`).join('')}</select></label><label class="full"><span>${t('Gift card code','Código de la gift card')} *</span><input id="gcCode" maxlength="40" value="${giftCode()}"></label></div>`,`<button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit" id="gcSave">${t('Create gift card','Crear gift card')}</button>`);
  setTimeout(()=>el('gcClient').focus(),50);
  form.onsubmit=async e=>{
    e.preventDefault();
    const payload={clientName:el('gcClient').value.trim(),address:el('gcAddress').value.trim(),firm:el('gcFirm').value.trim(),amount:Number(el('gcAmount').value),code:el('gcCode').value.trim().toUpperCase(),status:'Issued'};
    if(payload.clientName.length<2){show(t('Write the client name.','Escribe el nombre del cliente.'));el('gcClient').focus();return}
    if(payload.code.length<4){show(t('Write the gift card code.','Escribe el código de la gift card.'));el('gcCode').focus();return}
    const done=busyButton(el('gcSave'));
    try{await postJson('/api/safe/gift-cards',payload);await refreshHub(false);el('contactModal').classList.remove('open');render();toast(`✓ ${t('Gift card created','Gift card creada')}: ${payload.code} · ${money(payload.amount)}`,5000)}
    catch(err){show(err.message)}
    finally{done()}
  };
}
function referralsView(){
  const mine=(state.hub?.giftCards||[]).map(c=>({...c,by:'Management'}));
  const realtor=(state.realtorGifts||[]).map(c=>({...c,status:'Issued',by:c.realtorEmail}));
  const all=[...mine,...realtor].sort((a,b)=>String(b.createdAt||'').localeCompare(String(a.createdAt||'')));
  const issued=all.reduce((s,c)=>s+(Number(c.amount)||0),0);
  const rows=all.length?all.map(c=>`<tr><td><strong class="mono">${esc(c.code||c.publicId||'—')}</strong></td><td><strong>${esc(c.clientName||'—')}</strong>${c.address?`<small class="person-sub">${esc(c.address)}</small>`:''}</td><td>${esc(c.firm||'—')}</td><td><strong>${money(c.amount)}</strong></td><td>${esc(c.by)}</td><td>${c.createdAt?esc(whenText(c.createdAt)):'—'}</td></tr>`).join(''):`<tr><td colspan="6">${t('No gift cards yet. Use “Create gift card”.','Aún no hay gift cards. Usa “Crear gift card”.')}</td></tr>`;
  return `${pageHead('PARTNER PROGRAM','Closing gifts and eligible referral activity','Regalos de cierre y referidos elegibles','Gift cards created by Management and by Realtors in their portal, in one list.','Las gift cards creadas por Management y por los Realtors en su portal, en una sola lista.',`<button class="button primary" data-action="new-gift">+ ${t('Create gift card','Crear gift card')}</button>`)}<div class="grid cols-3">${metric(t('GIFT CARDS','GIFT CARDS'),String(all.length),money(issued)+' '+t('issued','emitidos'))}${metric(t('FROM REALTORS','DE REALTORS'),String(realtor.length),t('Created in the Realtor portal','Creadas en el portal del Realtor'))}${metric(t('POST-CLOSING JOBS','TRABAJOS POST-CIERRE'),String(state.requests.filter(r=>/post-closing/i.test(r.type||'')).length),t('Eligible referrals','Referidos elegibles'),true)}</div><section class="card table-card section-gap"><table class="table"><thead><tr><th>${t('Code','Código')}</th><th>${t('Client','Cliente')}</th><th>${t('Realtor or firm','Realtor o firma')}</th><th>${t('Amount','Monto')}</th><th>${t('Created by','Creada por')}</th><th>${t('Date','Fecha')}</th></tr></thead><tbody>${rows}</tbody></table></section>`;
}
const PORTALS={realtor:document.querySelector('meta[name="safe-realtor-portal"]')?.content||'http://localhost:5182',worker:document.querySelector('meta[name="safe-field-portal"]')?.content||'http://localhost:5183'};
const integrations=[
  ['RP','Realtor Portal',t('Requests, reports, estimate decisions and client messages.','Solicitudes, reportes, decisiones y mensajes.'),t('Open preview','Abrir vista'),t('Shared contract ready','Contrato compartido listo'),PORTALS.realtor],
  ['FT','Field Team Portal',t('Approved work orders, assignments, photos and completion.','Órdenes aprobadas, asignaciones, fotos y terminación.'),t('Open preview','Abrir vista'),t('Shared contract ready','Contrato compartido listo'),PORTALS.worker],
  ['OA','Oscar Lead Engine',t('Qualified Realtor leads from Facebook and Instagram activity.','Leads de Realtors desde actividad en Facebook e Instagram.'),t('Awaiting endpoint','Esperando endpoint'),t('Not live','No conectado')],
  ['G','Gmail',t('Inbound/outbound messages attached to a client and property.','Mensajes guardados con cliente y propiedad.'),t('Authorization required','Requiere autorización'),t('Not live','No conectado')],
  ['WA','WhatsApp Business',t('One-to-one updates, replies and approved templates through the official Business API.','Actualizaciones, respuestas y templates por la API oficial.'),t('Business setup required','Requiere configuración Business'),t('Not live','No conectado')],
  ['M','Meta',t('Facebook and Instagram lead activity and approved outreach.','Actividad de leads y mensajes aprobados.'),t('Permissions required','Requiere permisos'),t('Not live','No conectado')],
  ['S','SMS',t('Transactional updates, reminders and one-to-one follow-up.','Actualizaciones, recordatorios y seguimiento.'),t('Provider required','Requiere proveedor'),t('Not live','No conectado')]
];
function integrationsView(){const job=jobs()[0];const rec=job?`<h2>${esc(job.requestId||'')} · ${esc(job.address||'')}</h2></div><span class="tag green">${esc(woStatusLabel(job.status))}</span></div><div class="connection-list"><div class="connection-row"><div><strong>${esc(job.address||'—')}</strong><small>${t('Property used by Realtor, Management and Worker.','Propiedad usada por Realtor, Management y trabajador.')}</small></div><span class="tag aqua">Property</span></div><div class="connection-row"><div><strong>${esc(job.id||'—')}</strong><small>${t('Approved tasks, crew, schedule and field updates.','Tareas, equipo, agenda y progreso.')}</small></div><span class="tag navy">Work order</span></div><div class="connection-row"><div><strong>${esc(job.threadId||'—')}</strong><small>${t('Shared job thread.','Conversación compartida del trabajo.')}</small></div><span class="tag green">Thread</span></div></div>`:`<h2>${t('No live record yet','Aún no hay expediente')}</h2></div></div><p>${t('Send a request from the Realtor portal to create the shared IDs.','Envía una solicitud desde el portal del Realtor para crear los IDs.')}</p>`;return `${pageHead('SYSTEM CONNECTIONS','How the complete SAFE ecosystem communicates','Cómo se comunica el ecosistema SAFE','Three separate portals use the same IDs, workflow and approved price version.','Tres portales separados utilizan los mismos IDs, flujo y versión de precios.',`<button class="button secondary" data-action="view-contract">${t('View shared contract','Ver contrato compartido')}</button>`)}
  <section class="card"><div class="card-head"><div><span class="eyebrow">INDORDB</span><h2>${state.hubOnline?t('Shared database connected','Base de datos compartida conectada'):t('Shared database not reachable','No hay conexión con la base de datos compartida')}</h2></div><span class="tag ${state.hubOnline?'green':'red'}">${state.hubOnline?t('Online','En línea'):t('Offline','Sin conexión')}</span></div><p>${state.hubOnline?t('The three portals read and write the same IndorDB records.','Los tres portales leen y escriben los mismos registros de IndorDB.'):t('Forms cannot save until this site can reach IndorDB. Check the DefaultConnection connection string on the server.','Los formularios no pueden guardar hasta que este sitio llegue a IndorDB. Revisa la cadena de conexión DefaultConnection en el servidor.')}</p></section>
  <section class="card section-gap"><div class="card-head"><div><span class="eyebrow">${t('THREE PORTALS','TRES PORTALES')}</span><h2>${t('Separate access, one operating record','Accesos separados, un expediente operativo')}</h2></div><span class="tag green">IndorDB</span></div><div class="connection-map"><div class="system-node"><strong>${t('Realtor Portal','Portal del Realtor')}</strong><p>${t('Creates requests and receives approved client-facing updates.','Crea solicitudes y recibe actualizaciones aprobadas.')}</p><a class="button secondary small" href="${PORTALS.realtor}" target="_blank" rel="noopener">${t('Open Realtor portal','Abrir portal Realtor')}</a></div><div class="system-node center"><strong>SAFE Management</strong><p>${t('Owns approval, prices, assignments, invoices, permissions and audit history.','Controla aprobaciones, precios, asignaciones, invoices, permisos y auditoría.')}</p><span class="tag green">${t('SOURCE OF TRUTH','FUENTE PRINCIPAL')}</span></div><div class="system-node"><strong>${t('Field Team Portal','Portal del trabajador')}</strong><p>${t('Receives approved work and returns progress, photos and completion.','Recibe trabajo y devuelve progreso, fotos y terminación.')}</p><a class="button secondary small" href="${PORTALS.worker}" target="_blank" rel="noopener">${t('Open worker portal','Abrir portal trabajador')}</a></div></div></section>
  <section class="card section-gap"><div class="card-head"><div><span class="eyebrow">${t('ONE RECORD ACROSS ALL PORTALS','UN EXPEDIENTE EN LOS TRES PORTALES')}</span>${rec}</section>
  <div class="integration-grid section-gap">${integrations.map(x=>`<article class="integration-card"><span class="integration-icon">${x[0]}</span><div><h3>${x[1]}</h3><p>${x[2]}</p><div class="sync">${t('Status','Estado')}: ${x[4]}</div></div>${x[5]?`<a class="button ghost small" href="${x[5]}" target="_blank" rel="noopener">${x[3]}</a>`:`<button class="button secondary small" data-test-connection="${x[1]}">${x[3]}</button>`}</article>`).join('')}</div>
  <section class="card section-gap"><div class="card-head"><div><span class="eyebrow">${t('OSCAR INTEGRATION POINTS','PUNTOS PARA ÓSCAR')}</span><h2>${t('The minimum data every connection sends','Datos mínimos de cada conexión')}</h2></div></div><div class="connection-list"><div class="connection-row"><div><strong>POST /api/v1/realtor-requests</strong><small>${t('Creates request_id, contact_id, property_id and thread_id.','Crea request_id, contact_id, property_id y thread_id.')}</small></div><span class="tag green">Realtor → Management</span></div><div class="connection-row"><div><strong>PATCH /api/v1/requests/:id</strong><small>${t('Approved scope, price version, status and client-facing note.','Alcance aprobado, versión de precio, estado y nota.')}</small></div><span class="tag aqua">Management → Realtor</span></div><div class="connection-row"><div><strong>POST /api/v1/work-orders</strong><small>${t('Only Management can publish approved tasks, crew, materials and safety notes.','Solo Management publica tareas, equipo, materiales y seguridad.')}</small></div><span class="tag gold">Management → Field</span></div><div class="connection-row"><div><strong>POST /api/v1/jobs/:id/check-in</strong><small>${t('Emits worker.arrival_notified to the assigned Realtor and Management.','Emite worker.arrival_notified al Realtor asignado y Management.')}</small></div><span class="tag navy">Field → Realtor + Management</span></div><div class="connection-row"><div><strong>POST /api/v1/jobs/:id/access-instructions</strong><small>${t('Encrypted, masked instruction for the assigned worker with full audit history.','Instrucción cifrada y oculta para el trabajador, con auditoría.')}</small></div><span class="tag green">Realtor → Field + Management</span></div><div class="connection-row"><div><strong>POST /api/v1/integrations/oscar/leads</strong><small>${t('Facebook or Instagram source, public profile, business category, activity signal, consent and deduplication key.','Origen Facebook o Instagram, perfil público, categoría, señal, permiso y clave anti-duplicado.')}</small></div><span class="tag navy">Oscar → Management</span></div></div></section>`}
function openContractModal(){
  const portals=[[t('Realtor portal','Portal del Realtor'),PORTALS.realtor,t('Creates property requests, gift cards and client follow-ups.','Crea solicitudes de propiedades, gift cards y seguimientos de clientes.')],['SAFE Management',location.origin,t('Approves prices, estimates, invoices and crew assignments.','Aprueba precios, estimados, invoices y asignaciones.')],[t('Field Team portal','Portal del equipo de campo'),PORTALS.worker,t('Arrival, access, photos, changes, estimate requests and private messages.','Llegada, acceso, fotos, cambios, solicitudes de estimado y mensajes privados.')]];
  const ids=[['request_id',t('One property request keeps the same ID in the three portals.','Una solicitud de propiedad conserva el mismo ID en los tres portales.')],['work_order_id',t('The approved work the crew receives.','El trabajo aprobado que recibe el equipo.')],['thread_id',t('The shared job conversation.','La conversación compartida del trabajo.')],[t('catalog id (R0001…)','id del catálogo (R0001…)'),t('Every portal reads the same price version.','Todos los portales leen la misma versión de precios.')]];
  const flow=[t('Realtor sends the request','El Realtor envía la solicitud'),t('Management sets scope and price','Management define alcance y precio'),t('Client approves the estimate','El cliente aprueba el estimado'),t('Crew receives the work order','El equipo recibe la orden'),t('Arrival, photos and progress return to Management and the Realtor','Llegada, fotos y progreso regresan a Management y al Realtor')];
  modalShell('contractView','SHARED CONTRACT · v1',t('How the three portals share one record','Cómo comparten los tres portales un mismo expediente'),`<div class="contract-body"><h3>${t('Portals','Portales')}</h3>${portals.map(p=>`<div class="connection-row"><div><strong>${esc(p[0])}</strong><small>${esc(p[2])}</small></div><a class="button secondary small" href="${esc(p[1])}" target="_blank" rel="noopener">${t('Open','Abrir')} ↗</a></div>`).join('')}<h3>${t('Shared IDs','IDs compartidos')}</h3>${ids.map(x=>`<div class="connection-row"><div><strong class="mono">${esc(x[0])}</strong><small>${esc(x[1])}</small></div></div>`).join('')}<h3>${t('Workflow','Flujo')}</h3><ol class="contract-flow">${flow.map(f=>`<li>${esc(f)}</li>`).join('')}</ol><p class="muted-note">${t('All portals save directly to the same IndorDB database; no portal calls another portal.','Todos los portales guardan directamente en la misma base IndorDB; ningún portal llama a otro.')}</p></div>`,`<button type="button" class="button primary" data-contact-close>${t('Close','Cerrar')}</button>`);
}
async function testConnection(name){
  if(name==='Gmail'){
    const s=await getJson('/api/safe/email/status',{ready:false});state.emailReady=!!s.ready;
    toast(s.ready?t('Company email is connected and ready to send.','El correo de la compañía está conectado y listo para enviar.'):t('Company email is not configured yet: the server needs the Gmail app password.','El correo de la compañía aún no está configurado: falta la contraseña de aplicación de Gmail en el servidor.'),6000);return;
  }
  toast(t(`${name} is not connected yet. It needs the provider account and credentials before it can be turned on.`,`${name} aún no está conectado. Necesita la cuenta y las credenciales del proveedor antes de activarse.`),6000);
}
function openProfileModal(){
  const a=state.account||{};
  const {form,show}=modalShell('profileForm','SAFE MANAGEMENT',t('My profile','Mi perfil'),`<div class="form-grid"><label><span>${t('Full name','Nombre completo')} *</span><input id="pfName" value="${esc(a.name)}" maxlength="160"></label><label><span>${t('Email','Correo')}</span><input value="${esc(a.email)}" disabled></label><label><span>${t('Current password','Contraseña actual')}</span><input id="pfCurrent" type="password" autocomplete="current-password"></label><label><span>${t('New password (optional)','Nueva contraseña (opcional)')}</span><input id="pfNew" type="password" autocomplete="new-password"></label></div>`,`<button type="button" class="button secondary" id="pfSignOut">${t('Sign out','Cerrar sesión')}</button><button type="button" class="button secondary" data-contact-close>${t('Cancel','Cancelar')}</button><button class="button primary" type="submit">${t('Save profile','Guardar perfil')}</button>`);
  setTimeout(()=>el('pfName').focus(),50);
  el('pfSignOut').onclick=()=>{el('contactModal').classList.remove('open');leaveMgmt();toast(I18N.signedOut[state.lang==='en'?0:1])};
  form.onsubmit=e=>{
    e.preventDefault();
    const name=el('pfName').value.trim(),next=el('pfNew').value;
    if(name.length<2){show(t('Write your full name.','Escribe tu nombre completo.'));el('pfName').focus();return}
    const accounts=mgmtAccounts(),acct=accounts.find(x=>x.email.toLowerCase()===String(a.email||'').toLowerCase());
    if(next){
      if(!acct||acct.password!==el('pfCurrent').value){show(t('The current password is not correct.','La contraseña actual no es correcta.'));el('pfCurrent').focus();return}
      if(next.length<6){show(I18N.shortPassword[state.lang==='en'?0:1]);el('pfNew').focus();return}
      acct.password=next;
    }
    if(acct){acct.name=name;saveMgmtAccounts(accounts)}
    enterMgmt({name,email:a.email});el('contactModal').classList.remove('open');render();
    toast(`✓ ${t('Profile saved','Perfil guardado')}`);
  };
}
async function showFieldPhotos(id){
  const photos=await getJson(`/api/operations/field/activity/${id}/photos`,null);
  if(!photos){toast(t('Could not load the photos.','No se pudieron cargar las fotos.'));return}
  modalShell('photoView','FIELD TEAM',t('Photos from the field','Fotos del equipo de campo'),`<div class="photo-grid">${photos.map(p=>`<a href="${p}" target="_blank" rel="noopener"><img src="${p}" alt=""></a>`).join('')}</div>`,`<button type="button" class="button primary" data-contact-close>${t('Close','Cerrar')}</button>`);
}
function bindExtras(){
  const on=(sel,fn)=>document.querySelectorAll(sel).forEach(b=>b.onclick=()=>fn(b));
  on('[data-action="new-invoice"]',()=>openInvoiceModal());
  on('[data-invoice-for]',b=>openInvoiceModal(b.dataset.invoiceFor));
  on('[data-invoice-status]',b=>setInvoiceStatus(b,b.dataset.invoiceStatus,b.dataset.status));
  on('[data-action="new-gift"]',()=>openGiftModal());
  on('[data-action="view-contract"]',()=>openContractModal());
  on('[data-field-chat]',b=>{state.fieldChat=b.dataset.fieldChat;render()});
  on('[data-chat-with]',b=>{state.fieldChat=b.dataset.chatWith;navigate('messages')});
  on('[data-field-photos]',b=>showFieldPhotos(b.dataset.fieldPhotos));
  on('[data-field-convert]',async b=>{
    const done=busyButton(b,t('Sending…','Enviando…'));
    try{const created=await postJson(`/api/operations/field/requests/${b.dataset.fieldConvert}/convert`,{});state.localRequests=[created,...(state.localRequests||[])];mergeRequests();await loadField();render();toast(`✓ ${t('Request created in the intake queue','Solicitud creada en la cola')}: ${created.id}`,5000)}
    catch(err){done();toast(err.message)}
  });
  on('[data-field-status]',async b=>{
    const done=busyButton(b);
    try{await postJson(`/api/operations/field/requests/${b.dataset.fieldStatus}`,{status:b.dataset.status},'PATCH');await loadField();render();toast(t('Request updated · the worker sees the new status','Solicitud actualizada · el trabajador ve el nuevo estado'))}
    catch(err){done();toast(err.message)}
  });
  on('[data-action="field-reply"]',async b=>{
    const input=el('fieldReply'),body=input.value.trim();
    if(!body){input.focus();return}
    const done=busyButton(b,t('Sending…','Enviando…'));
    try{await postJson('/api/operations/field/messages',{worker:state.fieldChat,author:state.account?.name||'Management',body});await loadField();render();toast(t('Reply sent · the worker sees it in their portal','Respuesta enviada · el trabajador la ve en su portal'))}
    catch(err){done();toast(err.message)}
  });
  document.querySelectorAll('[data-wo-progress]').forEach(s=>s.onchange=async()=>{
    const progress=Number(s.value);s.disabled=true;
    try{await SafeApi.patch('/api/safe/work-orders/'+encodeURIComponent(s.dataset.woProgress),{progress,status:progress>=100?'completed':progress>0?'in_progress':undefined});await refreshHub(false);render();toast(t(`Progress saved · ${progress}%`,`Progreso guardado · ${progress}%`))}
    catch{s.disabled=false;toast(t('Could not save the progress.','No se pudo guardar el progreso.'))}
  });
  const emailForm=el('emailForm');
  if(emailForm)emailForm.onsubmit=async e=>{
    e.preventDefault();
    const to=el('emailTo').value,subject=el('emailSubject').value.trim(),body=el('emailBody').value.trim();
    if(!to){toast(t('Add a Realtor or client with an email first.','Primero agrega un Realtor o cliente con correo.'));return}
    if(!subject||!body){toast(t('Write the subject and the message.','Escribe el asunto y el mensaje.'));return}
    const done=busyButton(el('emailSend'),t('Sending…','Enviando…'));
    try{await postJson('/api/safe/email',{to,subject,body,replyTo:state.account?.email,replyName:state.account?.name});emailForm.reset();toast(`✓ ${t('Email sent to','Correo enviado a')} ${to}`,5000)}
    catch(err){toast(err.status===503?t('Company email is not configured yet. Ask the administrator to add the Gmail app password.','El correo de la compañía aún no está configurado. Pide al administrador agregar la contraseña de aplicación de Gmail.'):err.message,6000)}
    finally{done()}
  };
  const log=el('fieldChatLog');if(log)log.scrollTop=log.scrollHeight;
}
function render(){const views={overview:overviewView,intake:intakeView,approvals:approvalsView,realtors:realtorsView,leads:leadsView,clients:clientsView,messages:messagesView,estimates:estimatesView,projects:projectsView,assignments:assignmentsView,invoices:invoicesView,catalog:catalogView,promotions:promotionsView,referrals:referralsView,integrations:integrationsView};el('main').innerHTML=(views[state.route]||overviewView)();document.title=`SAFE Management — ${(routeTitles[state.route]||routeTitles.overview)[state.lang==='en'?0:1]}`;document.querySelectorAll('.nav button').forEach(b=>b.classList.toggle('active',b.dataset.route===state.route));el('backButton').classList.toggle('inactive',state.route==='overview');const setCount=(route,n)=>{const em=document.querySelector(`.nav [data-route="${route}"] em`);if(em)em.textContent=String(n)};setCount('intake',state.requests.length);setCount('approvals',state.requests.filter(r=>/pricing|review|new/i.test(r.status||'')).length);setCount('leads',liveLeads().length);setCount('messages',fieldWorkers().filter(w=>{const th=fieldThread(w.key),last=th[th.length-1];return last&&last.kind!=='reply'}).length);const nb=document.querySelector('#notificationButton b');if(nb)nb.textContent=String(state.requests.filter(r=>r.priority==='high').length);bindPage()}
function bindPage(){document.querySelectorAll('[data-route]').forEach(b=>{if(!b.closest('.nav')&&!b.closest('.brand'))b.onclick=()=>navigate(b.dataset.route)});document.querySelectorAll('[data-action="new-request"]').forEach(b=>b.onclick=()=>{el('newRequestForm').reset();resetRequestWarning();const due=el('newDue');if(!due.value||due.value<isoDate(0))due.value=isoDate(3);due.min=isoDate(0);el('newRequestModal').classList.add('open');setTimeout(()=>el('newAddress').focus(),50)});document.querySelectorAll('[data-assign-wo]').forEach(b=>b.onclick=async()=>{const workerId=Number(document.querySelector(`[data-assign-select="${CSS.escape(b.dataset.assignWo)}"]`)?.value);if(!workerId){toast(t('Choose the worker first','Primero escoge el trabajador'));return}try{await SafeApi.patch('/api/safe/work-orders/'+encodeURIComponent(b.dataset.assignWo),{workerId,status:'assigned'});if(b.dataset.assignReq)await SafeApi.patch('/api/safe/requests/'+encodeURIComponent(b.dataset.assignReq),{status:'Ready to assign',owner:'Operations'});await refreshHub(true);toast(t('Crew assigned · worker portal can see this job','Equipo asignado · el trabajador ya ve este trabajo'))}catch(e){toast(t('Could not assign — check IndorDB','No se pudo asignar — revisa IndorDB'))}});document.querySelectorAll('[data-action="assign-preview"]').forEach(b=>b.onclick=()=>{const first=jobs().find(j=>!j.workerId||/pending/i.test(j.status||''));if(first){const btn=document.querySelector(`[data-assign-wo="${first.id}"]`);if(btn)btn.click();else toast(t('Open Assignments to pick a crew','Abre Asignaciones para elegir equipo'))}else toast(t('No pending work order to assign','No hay orden pendiente'))});document.querySelectorAll('[data-action="message-preview"]').forEach(b=>b.onclick=()=>{navigate('messages');toast(t('Shared job thread opened','Conversación compartida abierta'))});document.querySelectorAll('[data-view-access]').forEach(b=>b.onclick=async()=>{const id=b.dataset.viewAccess,out=el('ops-access-'+id);b.disabled=true;try{const res=await fetch(`/api/safe/work-orders/${encodeURIComponent(id)}/access`,{headers:{Accept:'application/json'}});const body=await res.json().catch(()=>({}));if(!res.ok){if(out)out.textContent=(state.lang==='es'&&body.errorEs)||body.error||t('The access code could not be loaded','No se pudo cargar el código de acceso');b.disabled=false;return}if(out)out.innerHTML=`<strong>${esc(body.instruction)}</strong> · ${esc(body.sentBy||'Realtor')}`;b.remove()}catch{if(out)out.textContent=t('The access code could not be loaded','No se pudo cargar el código de acceso');b.disabled=false}});document.querySelectorAll('[data-action="acknowledge-arrival"]').forEach(b=>b.onclick=async()=>{const job=jobs()[0];try{if(job)await SafeApi.post('/api/safe/messages',{requestId:job.requestId,threadId:job.threadId,senderRole:'Operations',senderName:'Management',channel:'job',body:t('Arrival acknowledged. Management is monitoring the shared thread.','Llegada confirmada. Management supervisa la conversación.'),sourceApp:'Operations'});await refreshHub(true)}catch{}toast(t('Arrival acknowledged; shared thread remains under Management supervision','Llegada confirmada; Management continúa supervisando'))});document.querySelectorAll('[data-action="ops-send"]').forEach(b=>b.onclick=async()=>{const input=el('opsMsgInput');const value=input?.value.trim();if(!value)return;const job=jobs()[0];try{await SafeApi.post('/api/safe/messages',{requestId:job?.requestId,threadId:job?.threadId,senderRole:'Operations',senderName:'Management',channel:'job',body:value,sourceApp:'Operations'});input.value='';await refreshHub(true);toast(t('Sent to Realtor and worker','Enviado al Realtor y al trabajador'))}catch{toast(t('Message not saved — IndorDB unavailable','No se guardó — IndorDB no disponible'))}});document.querySelectorAll('[data-action="new-promo"]').forEach(b=>b.onclick=openPromoModal);document.querySelectorAll('[data-add-contact]').forEach(b=>b.onclick=()=>addContact(b.dataset.addContact));document.querySelectorAll('[data-action="new-lead"]').forEach(b=>b.onclick=openLeadModal);document.querySelectorAll('[data-action="new-repair"]').forEach(b=>b.onclick=()=>openRepairModal());document.querySelectorAll('[data-edit-repair]').forEach(b=>b.onclick=()=>openRepairModal(b.dataset.editRepair));document.querySelectorAll('[data-action="export-catalog"]').forEach(b=>b.onclick=exportCatalog);document.querySelectorAll('[data-action="new-estimate"]').forEach(b=>b.onclick=()=>openEstimateModal());document.querySelectorAll('[data-estimate-for]').forEach(b=>b.onclick=()=>openEstimateModal(b.dataset.estimateFor));document.querySelectorAll('[data-estimate-view]').forEach(b=>b.onclick=()=>viewEstimate(b.dataset.estimateView));document.querySelectorAll('[data-estimate-status]').forEach(b=>b.onclick=()=>setEstimateStatus(b,b.dataset.estimateStatus,b.dataset.status));document.querySelectorAll('[data-lead-move]').forEach(b=>b.onclick=()=>moveLead(b,b.dataset.leadMove,b.dataset.stage));document.querySelectorAll('[data-request]').forEach(b=>b.onclick=()=>openRequest(b.dataset.request));document.querySelectorAll('[data-filter]').forEach(b=>b.onclick=()=>{state.filter=b.dataset.filter;render()});const search=el('catalogSearch');if(search)search.oninput=e=>{state.catalogQuery=e.target.value;render()};document.querySelectorAll('[data-test-connection]').forEach(b=>b.onclick=()=>testConnection(b.dataset.testConnection));bindExtras()}
function requestSubmitButton(){return el('newRequestForm').querySelector('button[type="submit"]')}
function resetRequestWarning(){const form=el('newRequestForm');delete form.dataset.confirmDuplicate;form.querySelector('.form-warning')?.remove();const b=requestSubmitButton();if(!b.disabled)b.textContent=t('Create request','Crear solicitud')}
async function submitNewRequest(e){
  e.preventDefault();
  const form=e.target,button=requestSubmitButton();
  if(form.dataset.saving)return;
  const address=el('newAddress').value.trim(),contact=el('newContact').value.trim();
  const norm=s=>s.toLowerCase().replace(/\s+/g,' ').trim();
  const dup=state.requests.find(r=>norm(r.address||'')===norm(address)&&!isApprovedRequest(r));
  if(dup&&!form.dataset.confirmDuplicate){
    form.dataset.confirmDuplicate='1';
    form.querySelector('.form-warning')?.remove();
    form.querySelector('.modal-actions').insertAdjacentHTML('beforebegin',`<div class="form-warning">⚠ ${t(`There is already an open request for this address (${dup.id}, ${statusText(dup.status)}). Create another one anyway?`,`Ya existe una solicitud abierta para esta dirección (${dup.id}, ${statusText(dup.status)}). ¿Crear otra de todas formas?`)} <button type="button" class="link-button" data-open-dup="${esc(dup.id)}">${t('Open existing','Abrir la existente')}</button></div>`);
    form.querySelector('[data-open-dup]').onclick=()=>{el('newRequestModal').classList.remove('open');resetRequestWarning();navigate('intake');openRequest(dup.id)};
    button.textContent=t('Create anyway','Crear de todas formas');
    return;
  }
  form.dataset.saving='1';button.disabled=true;button.textContent=t('Creating…','Creando…');
  const payload={address,city:'Charlotte Area',contact,client:contact,source:el('newSource').value,sourceApp:'Operations',type:el('newDetails').value.trim()||'Management review',dueAt:el('newDue').value||null,items:0,value:0};
  let created=null;
  if(state.hubOnline){try{created=await SafeApi.post('/api/safe/requests',payload);await refreshHub(false)}catch{}}
  if(!created){
    try{
      const res=await fetch('/api/operations/requests',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({address,contact,source:payload.source,type:payload.type,due:payload.dueAt})});
      const body=await res.json().catch(()=>({}));
      if(!res.ok)throw new Error(apiError(body)||'');
      created=body;state.localRequests=[created,...(state.localRequests||[])];mergeRequests();
    }catch(err){
      delete form.dataset.saving;button.disabled=false;button.textContent=t('Create request','Crear solicitud');
      form.querySelector('.form-warning')?.remove();
      form.querySelector('.modal-actions').insertAdjacentHTML('beforebegin',`<div class="form-warning">⚠ ${esc(err.message||t('Could not save the request. Try again.','No se pudo guardar la solicitud. Intenta de nuevo.'))}</div>`);
      return;
    }
  }
  delete form.dataset.saving;button.disabled=false;
  el('newRequestModal').classList.remove('open');form.reset();resetRequestWarning();
  state.justCreated=created?.id;state.filter='all';navigate('intake');
  toast(t(`✓ Request ${created?.id||''} created for ${address}`,`✓ Solicitud ${created?.id||''} creada para ${address}`),5000);
  document.querySelector('.request-card.just-created')?.scrollIntoView({behavior:'smooth',block:'center'});
  setTimeout(()=>{state.justCreated=null;document.querySelector('.request-card.just-created')?.classList.remove('just-created')},6000);
}
async function updateRequest(r,changes,button,busyText,doneText){
  el('drawerActions').querySelectorAll('button').forEach(b=>b.disabled=true);if(button)button.textContent=busyText;
  const next={status:r.status,owner:r.owner,approvedAt:r.approvedAt??null,...changes};
  try{
    if(r.origin==='local'){
      const res=await fetch('/api/operations/requests/'+encodeURIComponent(r.id),{method:'PATCH',headers:{'Content-Type':'application/json'},body:JSON.stringify(next)});
      if(!res.ok)throw new Error();
      const saved=await res.json();
      state.localRequests=(state.localRequests||[]).map(x=>x.id===r.id?saved:x);mergeRequests();
    }else{
      await SafeApi.patch('/api/safe/requests/'+r.id,{status:next.status,owner:next.owner});await refreshHub(false);
      const fresh=state.requests.find(x=>x.id===r.id);if(fresh&&changes.approvedAt)fresh.approvedAt=changes.approvedAt;
    }
  }catch{
    render();openRequest(r.id);toast(t('Could not save the change. Try again.','No se pudo guardar el cambio. Intenta de nuevo.'),5000);return;
  }
  render();openRequest(r.id);toast(doneText);
}
function renderRequestActions(r){
  const box=el('drawerActions');
  if(isApprovedRequest(r)){
    box.innerHTML=`<div class="drawer-done"><span>✓</span><div><strong>${t('Already approved and sent','Ya fue aprobada y enviada')}</strong><small>${r.origin==='local'?t('Saved in SAFE Management. This request was created here, so the Realtor portal does not show it.','Guardada en SAFE Management. Esta solicitud se creó aquí, por eso no aparece en el portal del Realtor.'):t('The Realtor portal already has this update.','El portal del Realtor ya tiene esta actualización.')}</small></div></div><button class="button secondary" data-drawer-action="reopen">${t('Reopen review','Volver a revisión')}</button>`;
    box.querySelector('[data-drawer-action="reopen"]').onclick=e=>{if(!confirm(t('Reopen this request for review? The approval will be removed.','¿Volver a revisión esta solicitud? Se quitará la aprobación.')))return;updateRequest(r,{status:'Review',owner:'Management',approvedAt:null},e.currentTarget,t('Saving…','Guardando…'),t('Request returned to review','Solicitud regresada a revisión'))};
    return;
  }
  const assigned=isAssignedRequest(r);
  box.innerHTML=`<button class="button secondary" data-drawer-action="assign" ${assigned?'disabled':''}>${assigned?`✓ ${t('Assigned to','Asignada a')} ${esc(r.owner)}`:t('Assign owner','Asignar responsable')}</button><button class="button primary" data-drawer-action="approve">${t('Approve and send','Aprobar y enviar')}</button>`;
  box.querySelector('[data-drawer-action="assign"]').onclick=e=>updateRequest(r,{owner:'Operations',status:'Ready to assign'},e.currentTarget,t('Assigning…','Asignando…'),t('Request assigned to Operations','Solicitud asignada a Operaciones'));
  box.querySelector('[data-drawer-action="approve"]').onclick=e=>updateRequest(r,{status:'Ready to send',owner:'Management',approvedAt:new Date().toISOString()},e.currentTarget,t('Sending…','Enviando…'),r.origin==='local'?t('✓ Request approved and saved','✓ Solicitud aprobada y guardada'):t('✓ Approved and sent to the Realtor portal','✓ Aprobada y enviada al portal del Realtor'));
}
function openRequest(id){const r=state.requests.find(x=>x.id===id);if(!r)return;el('drawerEyebrow').textContent=r.id;el('drawerTitle').textContent=r.address;el('drawerBody').innerHTML=`<div class="detail-block"><div class="detail-row"><span>${t('Status','Estado')}</span>${statusTag(r.status)}</div><div class="detail-row"><span>${t('Priority','Prioridad')}</span>${priorityCell(r.priority)}</div><div class="detail-row"><span>${t('Due','Fecha')}</span><strong>${esc(r.due)}</strong></div></div><div class="detail-block"><strong>${t('Request from','Solicitud de')} ${esc(r.contact)}</strong><p>${esc(r.type)} · ${esc(r.source)}</p><small>${r.items} ${t('repair items attached','reparaciones adjuntas')}</small></div><div class="detail-block"><strong>${t('Management review','Revisión de Management')}</strong><p>${t('Confirm the repair scope, approved client-facing price and whether a site visit or specialist is required.','Confirma alcance, precio aprobado y si requiere visita o especialista.')}</p></div><div class="timeline"><div><strong>${t('Request received','Solicitud recibida')}</strong><small>${esc(r.source)}</small></div><div><strong>${t('Catalog matched','Catálogo relacionado')}</strong><small>${r.items} ${t('possible repair scenarios','posibles reparaciones')}</small></div><div>${isApprovedRequest(r)?`<strong>✓ ${t('Approved and sent to the Realtor portal','Aprobada y enviada al portal del Realtor')}</strong><small>${esc(r.owner||'Management')}${r.approvedAt?' · '+esc(new Date(r.approvedAt).toLocaleString(state.lang==='es'?'es-US':'en-US',{dateStyle:'short',timeStyle:'short'})):''}</small>`:isAssignedRequest(r)?`<strong>${t('Assigned, approval pending','Asignada, aprobación pendiente')}</strong><small>${esc(r.owner)}</small>`:`<strong>${t('Management decision pending','Decisión pendiente')}</strong><small>${esc(r.owner)}</small>`}</div></div>`;renderRequestActions(r);el('detailDrawer').classList.add('open');el('detailDrawer').setAttribute('aria-hidden','false');el('overlay').classList.add('open')}
function showSearch(q){const box=el('searchResults');if(!q.trim()){box.classList.remove('open');box.innerHTML='';return}const term=q.toLowerCase();const modules=moduleIndex.filter(m=>m.slice(2).join(' ').toLowerCase().includes(term)).map(m=>({route:m[0],icon:m[1],title:t(m[2],m[3]),sub:t('Management section','Sección de Management')}));const reqs=state.requests.filter(r=>Object.values(r).join(' ').toLowerCase().includes(term)).slice(0,5).map(r=>({route:'intake',icon:'⇥',title:r.address,sub:`${r.id} · ${r.status}`,request:r.id}));const repairs=state.catalog.filter(r=>[r.service,r.category,r.customer_en,r.customer_es].join(' ').toLowerCase().includes(term)).slice(0,5).map(r=>({route:'catalog',icon:'$',title:r.service,sub:r.category}));const all=[...modules,...reqs,...repairs].slice(0,12);box.innerHTML=all.length?all.map(x=>`<button class="search-result" data-search-route="${x.route}" data-search-request="${x.request||''}"><span>${x.icon}</span><span><strong>${esc(x.title)}</strong><small>${esc(x.sub)}</small></span><b>→</b></button>`).join(''):`<div style="padding:20px;text-align:center;color:var(--muted)">${t('No result yet','Sin resultados')}</div>`;box.classList.add('open');box.querySelectorAll('[data-search-route]').forEach(b=>b.onclick=()=>{box.classList.remove('open');el('globalSearch').value='';if(b.dataset.searchRequest){navigate('intake');setTimeout(()=>openRequest(b.dataset.searchRequest),40)}else navigate(b.dataset.searchRoute)})}
function setup(){
  showMgmtGate('choices');
  el('mgmtShowCreate').onclick=()=>showMgmtGate('create');
  el('mgmtShowSignIn').onclick=()=>showMgmtGate('signin');
  el('mgmtCreateToSignIn').onclick=()=>showMgmtGate('signin');
  el('mgmtSignInToCreate').onclick=()=>showMgmtGate('create');
  el('mgmtLang').onclick=()=>{state.lang=state.lang==='en'?'es':'en';localStorage.setItem('safe-management-lang',state.lang);applyLanguage()};
  el('mgmtSignOut').onclick=()=>{leaveMgmt();toast(I18N.signedOut[state.lang==='en'?0:1])};
  el('adminProfile').onclick=openProfileModal;
  el('mgmtGateForm').onsubmit=e=>{
    e.preventDefault();
    if(!el('mgmtCreate').hidden){
      const name=el('mgmtCreateName').value.trim();
      const email=el('mgmtCreateEmail').value.trim();
      const password=el('mgmtCreatePassword').value;
      if(!name){toast(I18N.fullName[state.lang==='en'?0:1]);return}
      if(!validEmail(email)){toast(I18N.badEmail[state.lang==='en'?0:1]);return}
      if(password.length<6){toast(I18N.shortPassword[state.lang==='en'?0:1]);return}
      const accounts=mgmtAccounts();
      if(accounts.some(a=>a.email.toLowerCase()===email.toLowerCase())){toast(I18N.accountExists[state.lang==='en'?0:1]);showMgmtGate('signin');el('mgmtSignInEmail').value=email;return}
      accounts.push({name,email,password});
      saveMgmtAccounts(accounts);
      if(window.SafeApi)SafeApi.post('/api/safe/clients',{name,email,role:'Management'}).catch(()=>{});
      enterMgmt({name,email});
      toast(I18N.profileReady[state.lang==='en'?0:1]);
      return;
    }
    const email=el('mgmtSignInEmail').value.trim();
    const password=el('mgmtSignInPassword').value;
    if(!validEmail(email)){toast(I18N.badEmail[state.lang==='en'?0:1]);return}
    const found=mgmtAccounts().find(a=>a.email.toLowerCase()===email.toLowerCase()&&a.password===password);
    if(!found){toast(I18N.noAccount[state.lang==='en'?0:1]);return}
    enterMgmt(found);
    toast(I18N.signedIn[state.lang==='en'?0:1]);
  };
  document.querySelectorAll('.nav [data-route]').forEach(b=>b.onclick=()=>navigate(b.dataset.route));document.querySelector('.brand').onclick=()=>navigate('overview');el('backButton').onclick=goBack;el('menuToggle').onclick=()=>{const open=!el('sidebar').classList.contains('open');el('sidebar').classList.toggle('open',open);el('overlay').classList.toggle('open',open);el('menuToggle').setAttribute('aria-expanded',String(open))};el('overlay').onclick=closeLayers;el('closeDrawer').onclick=closeLayers;el('languageButton').onclick=()=>{state.lang=state.lang==='en'?'es':'en';localStorage.setItem('safe-management-lang',state.lang);applyLanguage();render()};document.querySelectorAll('[data-close="modal"]').forEach(b=>b.onclick=()=>b.closest('.modal').classList.remove('open'));document.querySelectorAll('.modal').forEach(m=>m.onclick=e=>{if(e.target===m)m.classList.remove('open')});el('newRequestForm').onsubmit=submitNewRequest;document.querySelectorAll('#newRequestForm input,#newRequestForm select,#newRequestForm textarea').forEach(i=>i.addEventListener('input',()=>resetRequestWarning()));el('globalSearch').oninput=e=>showSearch(e.target.value);el('globalSearch').onfocus=e=>showSearch(e.target.value);document.addEventListener('click',e=>{if(!el('globalSearchWrap').contains(e.target))el('searchResults').classList.remove('open')});document.addEventListener('keydown',e=>{if((e.metaKey||e.ctrlKey)&&e.key.toLowerCase()==='k'){e.preventDefault();el('globalSearch').focus()}if(e.key==='Escape'){closeLayers();document.querySelectorAll('.modal.open').forEach(m=>m.classList.remove('open'))}})}
function registerWebMCP(){const ctx=document.modelContext;if(!ctx?.registerTool)return;const register=tool=>{try{void Promise.resolve(ctx.registerTool(tool)).catch(()=>{})}catch{}};register({name:'list_realtor_requests',title:'List Realtor requests',description:'Read the current SAFE Management intake queue, optionally filtered by status.',inputSchema:{type:'object',properties:{status:{type:'string'}},additionalProperties:false},annotations:{readOnlyHint:true,untrustedContentHint:false},execute(input){const status=String(input?.status||'').toLowerCase();return state.requests.filter(r=>!status||r.status.toLowerCase().includes(status)).map(r=>({id:r.id,address:r.address,status:r.status,due:r.due,owner:r.owner}))}});register({name:'trace_ecosystem_record',title:'Trace an ecosystem record',description:'Show the shared IDs connecting one request across Realtor, Management and Field Team portals.',inputSchema:{type:'object',properties:{requestId:{type:'string'}},required:['requestId'],additionalProperties:false},annotations:{readOnlyHint:true,untrustedContentHint:false},execute(input){const id=String(input.requestId||'');const r=state.requests.find(x=>x.id===id);const job=jobs().find(j=>j.requestId===id||j.id===id);if(!r&&!job)return {request_id:id,status:'not_found'};return {request_id:r?.id||id,address:r?.address||job?.address,work_order_id:job?.id||null,thread_id:job?.threadId||null,status:job?.status||r?.status}}});register({name:'update_request_status',title:'Update request status',description:'Update the visible status of one SAFE Management request.',inputSchema:{type:'object',properties:{requestId:{type:'string'},status:{type:'string'}},required:['requestId','status'],additionalProperties:false},annotations:{readOnlyHint:false,untrustedContentHint:false},execute(input){const r=state.requests.find(x=>x.id===input.requestId);if(!r)throw new Error('Request not found');r.status=String(input.status);persist();render();return {id:r.id,status:r.status}}})}
async function init(){applyLanguage();setup();let saved=null;try{saved=JSON.parse(sessionStorage.getItem('safe-management-session')||'null')}catch{}if(saved?.email&&saved?.name)enterMgmt(saved);else leaveMgmt();try{let response=await fetch('/api/operations/catalog');if(!response.ok)response=await fetch('repair-catalog.json');state.catalog=await response.json();el('catalogCount').textContent=state.catalog.length}catch{toast(t('Price catalog unavailable','Catálogo no disponible'))}await Promise.all([refreshHub(false),loadLocalContacts(),loadLocalLeads(),loadLocalRequests(),loadLocalEstimates(),loadField(),loadLocalInvoices(),loadRealtorGifts(),getJson('/api/safe/email/status',{ready:false}).then(s=>{state.emailReady=!!s.ready})]);render();registerWebMCP();setInterval(poll,12000)}
window.addEventListener('hashchange',()=>{const route=location.hash.replace('#','');if(route&&route!==state.route){state.route=route;render()}});
init();
