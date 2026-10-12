const copy={
      en:{gatewayEyebrow:'Your assigned work · One clear place',gatewayTitle:'Know what to do next, from arrival to completion.',gatewayCopy:'Open approved work orders, check in at the property, request access, message the assigned Realtor and Management, upload photos and request changes.',gatewayFoot:'English is the default language. Every worker can switch to Spanish.',signIn:'Field team sign in',workEmail:'Work email',password:'Password',continue:'Open my work',previewAs:'SAMPLE ONLY',management:'Management',worker:'Open sample view',realtor:'Realtor',previewNote:'The sample does not create an account and does not sign you in.',allPortals:'Sign out',goBack:'← Back',badEmail:'Enter a complete email, such as name@company.com',gateTitle:'SAFE Field Team',gateIntro:'Create a profile, or sign in if you already have one.',createProfile:'Create profile',signInExisting:'I already have an account',fullName:'Full name',haveAccount:'Already have an account? Sign in',needAccount:'No account yet? Create a profile',shortPassword:'Use at least 6 characters.',accountExists:'That email already has a profile. Sign in.',noAccount:'No profile matches that email and password.',profileReady:'Profile created. This is your account.',signedIn:'Signed in to your account.',sampleBanner:'Sample view',sampleCopy:'This is not an account. Create a profile if you need to sign in later.',accountBanner:'Your account',accountCopy:'Management assigns work to this profile.',accountEmpty:'Your profile is ready',accountEmptyCopy:'No work is assigned to this account yet.',previewEmpty:'Sample only',previewEmptyCopy:'This sample is not your account.',companyName:'Company / legal name',phone10:'Phone (10 digits)',specialties:'What work do you specialize in?',myProfile:'My profile',chooseSpecialty:'Choose at least one specialty.',badPhone:'The phone must have exactly 10 digits.',completeProfile:'Complete your profile: add your company and specialties.',profileSaved:'Profile saved.',saveFailed:'It could not be saved. Try again.'},
      es:{gatewayEyebrow:'Tu trabajo asignado · Un lugar claro',gatewayTitle:'Sabe qué hacer después, desde la llegada hasta terminar.',gatewayCopy:'Abre órdenes aprobadas, registra tu llegada, solicita acceso, escribe al Realtor asignado y a Management, sube fotos y solicita cambios.',gatewayFoot:'El idioma predeterminado es inglés. Cada trabajador puede cambiar a español.',signIn:'Acceso del equipo',workEmail:'Correo de trabajo',password:'Contraseña',continue:'Abrir mis trabajos',previewAs:'SOLO UN EJEMPLO',management:'Management',worker:'Abrir vista de ejemplo',realtor:'Realtor',previewNote:'La vista de ejemplo no crea una cuenta y no inicia sesión.',allPortals:'Cerrar sesión',goBack:'← Volver',badEmail:'Escribe un correo completo, como nombre@empresa.com',gateTitle:'SAFE Field Team',gateIntro:'Crea un perfil, o ingresa si ya tienes una cuenta.',createProfile:'Crear perfil',signInExisting:'Ya tengo una cuenta',fullName:'Nombre completo',haveAccount:'¿Ya tienes cuenta? Ingresar',needAccount:'¿No tienes cuenta? Crear perfil',shortPassword:'Usa al menos 6 caracteres.',accountExists:'Ese correo ya tiene un perfil. Ingresa.',noAccount:'No hay un perfil con ese correo y contraseña.',profileReady:'Perfil creado. Esta es tu cuenta.',signedIn:'Sesión iniciada en tu cuenta.',sampleBanner:'Vista de ejemplo',sampleCopy:'Esto no es una cuenta. Crea un perfil si necesitas ingresar después.',accountBanner:'Tu cuenta',accountCopy:'Management asigna el trabajo a este perfil.',accountEmpty:'Tu perfil está listo',accountEmptyCopy:'Todavía no hay trabajo asignado a esta cuenta.',previewEmpty:'Solo un ejemplo',previewEmptyCopy:'Este ejemplo no es tu cuenta.',companyName:'Empresa / razón social',phone10:'Teléfono (10 dígitos)',specialties:'¿En qué trabajos te especializas?',myProfile:'Mi perfil',chooseSpecialty:'Escoge al menos una especialidad.',badPhone:'El teléfono debe tener exactamente 10 dígitos.',completeProfile:'Completa tu perfil: agrega tu empresa y especialidades.',profileSaved:'Perfil guardado.',saveFailed:'No se pudo guardar. Intenta de nuevo.'}
    };
    const portalState={lang:'en',portal:null,wizard:1,channel:'message',hub:{workOrders:[],messages:[],contacts:[],properties:[],requests:[],invoices:[],events:[]},workOrderId:null,requestId:null};
    function applyHub(snap){
      portalState.hub=snap||portalState.hub;
      const assigned=(snap?.workOrders||[]).filter(j=>j.workerId||!/pending/i.test(j.status||''));
      const job=(assigned[0]||snap?.workOrders?.[0]);
      if(job){portalState.workOrderId=job.id;portalState.requestId=job.requestId;portalState.threadId=job.threadId}
    }
    async function refreshHub(rerender=false){
      if(!window.SafeApi){fillManagement();return false}
      try{const before=JSON.stringify([liveJobs(),liveMessages()]);applyHub(await SafeApi.get('/api/safe/snapshot?portal=core'));fillManagement();if(rerender&&portalState.portal==='worker'&&before!==JSON.stringify([liveJobs(),liveMessages()]))renderPortal('worker');return true}
      catch(e){console.warn('IndorDB snapshot unavailable',e);fillManagement();return false}
    }
    function liveJobs(){return portalState.hub?.workOrders||[]}
    function liveMessages(){return portalState.hub?.messages||[]}
    function workerIdentity(){if(portalState.mode==='account'&&portalState.account?.name)return portalState.account.name;const job=liveJobs()[0];if(job?.worker&&job.worker!=='Unassigned')return job.worker;return (portalState.hub?.contacts||[]).find(c=>c.role==='Worker')?.name||L('Field worker','Trabajador')}
    function moneyFmt(v){return new Intl.NumberFormat('en-US',{style:'currency',currency:'USD'}).format(Number(v)||0)}
    function fillEstimateSelects(){
      const clients=(portalState.hub?.contacts||[]).filter(c=>c.role==='Client');
      const props=portalState.hub?.properties||[];
      const cs=document.getElementById('estClient');
      const ps=document.getElementById('estProperty');
      if(cs)cs.innerHTML=clients.length?clients.map(c=>`<option value="${esc(c.name)}">${esc(c.name)}</option>`).join(''):'<option value="">Aún no hay clientes</option>';
      else {const sel=document.querySelector('#estimateModal select');if(sel)sel.innerHTML=clients.length?clients.map(c=>`<option value="${esc(c.name)}">${esc(c.name)}</option>`).join(''):'<option value="">Aún no hay clientes</option>'}
      if(ps)ps.innerHTML=props.length?props.map(p=>`<option value="${esc(p.address)}">${esc(p.address)}</option>`).join(''):'<option value="">Aún no hay propiedades</option>';
      else {const sels=document.querySelectorAll('#estimateModal select');if(sels[1])sels[1].innerHTML=props.length?props.map(p=>`<option value="${esc(p.address)}">${esc(p.address)}</option>`).join(''):'<option value="">Aún no hay propiedades</option>'}
    }
    function fillManagement(){
      const hub=portalState.hub||{};
      const jobs=hub.workOrders||[];
      const contacts=hub.contacts||[];
      const clients=contacts.filter(c=>c.role==='Client');
      const workers=contacts.filter(c=>c.role==='Worker');
      const props=hub.properties||[];
      const invoices=hub.invoices||[];
      const events=hub.events||[];
      const reqs=hub.requests||[];
      const sales=jobs.reduce((s,j)=>s+(Number(j.value)||0),0);
      const pending=reqs.filter(r=>/new|pricing|review|pending/i.test(r.status||''));
      const ar=invoices.filter(i=>!/paid/i.test(i.status||'')).reduce((s,i)=>s+(Number(i.total)||0),0);
      const set=(id,v)=>{const el=document.getElementById(id);if(el)el.textContent=v};
      set('dashSales',moneyFmt(sales));set('dashApprovals',String(pending.length));set('dashJobs',String(jobs.length));set('dashAR',moneyFmt(ar));set('dashPendingBadge',String(pending.length));
      set('invBilled',moneyFmt(invoices.reduce((s,i)=>s+(Number(i.total)||0),0)));set('invPaid',moneyFmt(invoices.filter(i=>/paid/i.test(i.status||'')).reduce((s,i)=>s+(Number(i.total)||0),0)));set('invOpen',moneyFmt(ar));set('invCount',String(invoices.length));
      const dashP=document.getElementById('dashProjectsBody');
      if(dashP)dashP.innerHTML=jobs.length?jobs.slice(0,8).map(j=>`<tr><td><div class="name">${esc(j.client||'—')}</div><div class="sub">${esc(j.address||'')}</div></td><td>${esc(j.scope||'')}</td><td><span class="badge blue">${esc(j.status||'')}</span></td><td class="mono name">${moneyFmt(j.value)}</td><td><button class="mini open-detail" data-id="${esc(j.id)}">Abrir</button></td></tr>`).join(''):'<tr><td colspan="5">Aún no hay proyectos en IndorDB.</td></tr>';
      const appr=document.getElementById('dashApprovalsBody');
      if(appr)appr.innerHTML=pending.length?pending.slice(0,5).map(r=>`<div class="approval"><div class="approval-icon">＋</div><div><b>${esc(r.type||'Request')}</b><p>${esc(r.address||'')} · ${esc(r.contact||'')}</p></div></div>`).join(''):'<p>Cuando un Realtor envíe una solicitud, aparece aquí.</p>';
      const act=document.getElementById('dashActivityBody');
      if(act)act.innerHTML=events.length?events.slice(0,6).map(e=>`<div class="activity"><div class="timeline"></div><div><b>${esc(e.eventName||'')}</b><p>${esc(e.payload||e.sourceApp||'')}</p></div></div>`).join(''):'<p>Los eventos de IndorDB aparecen aquí.</p>';
      const cb=document.getElementById('clientsBody');
      if(cb)cb.innerHTML=clients.length?clients.map(c=>{const p=props.find(x=>x.clientId===c.id);const job=jobs.find(j=>j.client===c.name);return `<tr><td><div class="name">${esc(c.name)}</div><div class="sub">${esc(c.role)}</div></td><td>${esc(c.phone||'—')}<div class="sub">${esc(c.email||'')}</div></td><td>${esc(p?.address||job?.address||'—')}</td><td><span class="badge blue">${esc(c.firm||'IndorDB')}</span></td><td>${job?esc(job.scope||job.status):'—'}</td><td></td></tr>`}).join(''):'<tr><td colspan="6">Aún no hay clientes. Usa Agregar cliente.</td></tr>';
      const pg=document.getElementById('propertiesGrid');
      if(pg)pg.innerHTML=props.length?props.map(p=>{const c=contacts.find(x=>x.id===p.clientId);return `<div class="info-tile"><div class="tile-top"><div><h3>${esc(p.address)}</h3><p>${esc(p.city||'')}</p></div><span class="badge green">${esc(p.status||'')}</span></div><div class="connection"><span>Cliente</span><b>${esc(c?.name||'—')}</b></div></div>`}).join(''):'<p>Aún no hay propiedades en IndorDB.</p>';
      const estimates=hub.estimates||[];
      const eb=document.getElementById('estimatesBody');
      if(eb){
        if(estimates.length)eb.innerHTML=estimates.map(e=>{const r=reqs.find(x=>x.dbId===e.requestId);return `<tr><td class="name mono">${esc(e.publicId||e.id)}</td><td><div class="name">${esc(r?.client||r?.contact||'—')}</div><div class="sub">${esc(r?.address||'')}</div></td><td>${esc(r?.type||e.rangeText||'')}</td><td><span class="badge blue">${esc(e.status||'')}</span></td><td class="name mono">${moneyFmt(e.total)}</td><td><button class="mini open-detail" data-id="${esc(r?.id||e.publicId)}">Abrir</button></td></tr>`}).join('');
        else if(reqs.length)eb.innerHTML=reqs.map(r=>`<tr><td class="name mono">${esc(r.id)}</td><td><div class="name">${esc(r.client||r.contact||'—')}</div><div class="sub">${esc(r.address||'')}</div></td><td>${esc(r.type||'')}</td><td><span class="badge blue">${esc(r.status||'')}</span></td><td class="name mono">${moneyFmt(r.value)}</td><td><button class="mini open-detail" data-id="${esc(r.id)}">Abrir</button></td></tr>`).join('');
        else eb.innerHTML='<tr><td colspan="6">Aún no hay estimates en IndorDB.</td></tr>';
      }
      const ib=document.getElementById('invoicesBody');
      if(ib)ib.innerHTML=invoices.length?invoices.map(i=>{const r=reqs.find(x=>x.dbId===i.requestId);return `<tr><td class="name mono">${esc(i.publicId||i.id)}</td><td>${esc(r?.client||r?.address||'—')}</td><td>—</td><td><span class="badge">${esc(i.status||'')}</span></td><td class="mono">${moneyFmt(i.total)}</td><td class="mono">${/paid/i.test(i.status||'')?'$0.00':moneyFmt(i.total)}</td><td></td></tr>`}).join(''):'<tr><td colspan="7">Aún no hay invoices en IndorDB.</td></tr>';
      const tg=document.getElementById('teamGrid');
      if(tg){
        if(workers.length)tg.innerHTML=workers.map(w=>{const job=jobs.find(j=>j.workerId===w.id||j.worker===w.name);return `<div class="info-tile"><div class="tile-top"><div class="big-icon">${esc((w.name||'?')[0])}</div><span class="badge blue">${esc(job?.status||'Disponible')}</span></div><h3>${esc(w.name)}</h3><p>${esc(job?.address||'Sin trabajo asignado')}</p></div>`}).join('');
        else if(jobs.length)tg.innerHTML=jobs.map(j=>`<div class="info-tile"><h3>${esc(j.worker||j.address||j.id)}</h3><p>${esc(j.status||'')}</p></div>`).join('');
        else tg.innerHTML='<p>Cuando Operations asigne un trabajador, aparece aquí.</p>';
      }
      if(!document.getElementById('dashProjectsBody')){
        const tb=document.querySelector('#view-dashboard tbody');
        if(tb)tb.innerHTML=jobs.length?jobs.slice(0,8).map(j=>`<tr><td><div class="name">${esc(j.client||'—')}</div><div class="sub">${esc(j.address||'')}</div></td><td>${esc(j.scope||'')}</td><td><span class="badge blue">${esc(j.status||'')}</span></td><td class="mono">${moneyFmt(j.value)}</td><td></td></tr>`).join(''):'<tr><td colspan="5">Aún no hay proyectos en IndorDB.</td></tr>';
        const stats=document.querySelectorAll('#view-dashboard .stat strong');
        if(stats[0])stats[0].textContent=moneyFmt(sales);if(stats[1])stats[1].textContent=String(pending.length);if(stats[2])stats[2].textContent=String(jobs.length);if(stats[3])stats[3].textContent=moneyFmt(ar);
        const bodies=document.querySelectorAll('#view-dashboard .card-body');
        if(bodies[0]&&!document.getElementById('dashApprovalsBody'))bodies[0].innerHTML=pending.length?pending.slice(0,5).map(r=>`<p><b>${esc(r.type||'Request')}</b> · ${esc(r.address||'')}</p>`).join(''):'<p>Cuando un Realtor envíe una solicitud, aparece aquí.</p>';
        if(bodies[1]&&!document.getElementById('dashActivityBody'))bodies[1].innerHTML=events.length?events.slice(0,6).map(e=>`<p><b>${esc(e.eventName||'')}</b> · ${esc(e.payload||'')}</p>`).join(''):'<p>Los eventos de IndorDB aparecen aquí.</p>';
      }
      if(!document.getElementById('clientsBody')){const tb=document.querySelector('#view-clients tbody');if(tb)tb.innerHTML=clients.length?'': '<tr><td colspan="6">Aún no hay clientes. Usa Agregar cliente.</td></tr>'}
      if(!document.getElementById('propertiesGrid')){const g=document.querySelector('#view-properties .section-grid');if(g)g.innerHTML=props.length?'':'<p>Aún no hay propiedades en IndorDB.</p>'}
      if(!document.getElementById('estimatesBody')){const tb=document.querySelector('#view-estimates tbody');if(tb)tb.innerHTML='<tr><td colspan="6">Aún no hay estimates en IndorDB.</td></tr>'}
      if(!document.getElementById('invoicesBody')){const tb=document.querySelector('#view-invoices tbody');if(tb)tb.innerHTML='<tr><td colspan="7">Aún no hay invoices en IndorDB.</td></tr>'}
      if(!document.getElementById('teamGrid')){const g=document.querySelector('#view-team .section-grid');if(g)g.innerHTML='<p>Cuando Operations asigne un trabajador, aparece aquí.</p>'}
      const drawerTitle=document.getElementById('drawerTitle')||document.querySelector('#estimateDrawer h2');
      if(drawerTitle&&/SPS-|Esperanza/i.test(drawerTitle.textContent||''))drawerTitle.textContent='—';
      const drawerP=document.getElementById('drawerSub')||document.querySelector('#estimateDrawer .drawer-head p');
      if(drawerP&&/Fox Thorne|Esperanza/i.test(drawerP.textContent||''))drawerP.textContent='Selecciona un registro';
      fillEstimateSelects();
      document.querySelectorAll('.open-detail').forEach(b=>b.onclick=()=>{
        const id=b.dataset.id;
        const j=jobs.find(x=>x.id===id)||reqs.find(x=>x.id===id);
        if(!j)return;
        const title=document.getElementById('drawerTitle');if(title)title.textContent=j.id||'';
        const sub=document.getElementById('drawerSub');if(sub)sub.textContent=`${j.client||j.contact||''} · ${j.address||''}`;
        const st=document.getElementById('drawerState');if(st)st.textContent=j.status||'';
        const tot=document.getElementById('drawerTotal');if(tot)tot.textContent=moneyFmt(j.value);
        const sc=document.getElementById('drawerScope');if(sc)sc.textContent=j.scope||j.type||'';
        const badge=document.getElementById('drawerStatus');if(badge)badge.textContent=j.status||'IndorDB';
        openDrawer();
      });
    }
    const L=(en,es)=>portalState.lang==='es'?es:en;
    let titles={dashboard:'Overview',clients:'Clients',properties:'Properties',estimates:'Estimates',invoices:'Invoices & payments',team:'Team & jobs',prices:'Price catalog',integrations:'Integrations'};
    function applyLanguage(lang){
      portalState.lang=lang;document.documentElement.lang=lang;
      document.querySelectorAll('[data-i18n]').forEach(el=>{const k=el.dataset.i18n;if(copy[lang][k])el.textContent=copy[lang][k]});
      document.querySelectorAll('.language-option').forEach(b=>b.classList.toggle('active',b.dataset.lang===lang));
      titles=lang==='es'?{dashboard:'Resumen',clients:'Clientes',properties:'Propiedades',estimates:'Estimates',invoices:'Invoices y pagos',team:'Equipo y trabajos',prices:'Catálogo de precios',integrations:'Integraciones'}:{dashboard:'Overview',clients:'Clients',properties:'Properties',estimates:'Estimates',invoices:'Invoices & payments',team:'Team & jobs',prices:'Price catalog',integrations:'Integrations'};
      translateManagement();renderCreateSpecialties();
      if(portalState.portal==='worker')renderPortal('worker');
    }
    function setButtonText(selector,text){const el=document.querySelector(selector);if(!el)return;const icon=el.querySelector('.nav-icon');el.innerHTML=icon?icon.outerHTML+text:text}
    function translateManagement(){
      const es=portalState.lang==='es';
      setButtonText('[data-view="dashboard"]',es?'Resumen':'Overview');setButtonText('[data-view="clients"]',es?'Clientes':'Clients');setButtonText('[data-view="properties"]',es?'Propiedades':'Properties');setButtonText('[data-view="estimates"]','Estimates');setButtonText('[data-view="invoices"]',es?'Invoices y pagos':'Invoices & payments');setButtonText('[data-view="team"]',es?'Equipo y trabajos':'Team & jobs');setButtonText('[data-view="prices"]',es?'Catálogo de precios':'Price catalog');setButtonText('[data-view="integrations"]',es?'Integraciones':'Integrations');
      document.querySelectorAll('.nav-label').forEach((n,i)=>n.textContent=i===0?(es?'Operaciones':'Operations'):(es?'Sistema':'System'));
      document.getElementById('crumbTitle').textContent=titles[document.querySelector('.nav button.active')?.dataset.view||'dashboard'];
      document.getElementById('globalSearch').placeholder=es?'Buscar cliente, dirección o invoice':'Search client, address or invoice';
      document.getElementById('quickEstimate').textContent=es?'＋ Nuevo estimate':'＋ New estimate';
      const h=document.querySelector('#view-dashboard h1');if(h)h.textContent=es?'Buenos días':'Good morning';
      const p=document.querySelector('#view-dashboard .view-head p');if(p)p.textContent=es?'Lo importante para operar SAFE hoy, en una sola vista.':'What matters for SAFE today, in one clear view.';
      document.querySelectorAll('.portal-home').forEach(b=>{const s=b.querySelector('[data-i18n]');if(!s)b.textContent=es?'Todos los portales':'All portals'});
      const pairs=[
        ['Ventas abiertas','Open sales'],['Por aprobar','Pending approval'],['Trabajos activos','Active jobs'],['Por cobrar','Accounts receivable'],['8 estimates activos','8 active estimates'],['Requieren tu revisión','Require your review'],['2 terminan hoy','2 finish today'],['4 invoices pendientes','4 pending invoices'],['Proyectos recientes','Recent projects'],['Ver todos','View all'],['Necesitan aprobación','Needs approval'],['3 pendientes','3 pending'],['Actividad reciente','Recent activity'],['Aprobado','Approved'],['En progreso','In progress'],['Revisar','Review'],['Borrador','Draft'],['Abrir','Open'],['Enviar','Send'],['Editar','Edit'],['Agregar cliente','Add client'],['Clientes','Clients'],['Propiedades','Properties'],['Equipo y trabajos','Team & jobs'],['Catálogo maestro de precios','Master price catalog'],['Integraciones','Integrations'],['Todos los estados','All statuses'],['Pagado','Paid'],['Listo para enviar','Ready to send'],['Pago parcial','Partial payment'],['Registrar pago','Record payment'],['Asignar trabajo','Assign job'],['Disponible','Available'],['Material pendiente','Material pending'],['Agregar precio','Add price'],['Dictar experiencia','Dictate experience'],['Todos los rubros','All trades'],['No conectado','Not connected'],['Preparar API','Prepare API'],['Futuro','Future'],['Vista de arquitectura','Architecture view'],['Sistema sincronizado','System synchronized'],['Cliente','Client'],['Propiedad','Property'],['Estado','Status'],['Total','Total'],['Proyecto','Project'],['Emitido','Issued'],['Saldo','Balance'],['Servicio','Service'],['Fuente','Source']
      ];
      const from=new Map(pairs.map(x=>[es?x[1]:x[0],es?x[0]:x[1]]));
      document.querySelectorAll('#managementApp h1,#managementApp h2,#managementApp h3,#managementApp p,#managementApp th,#managementApp label,#managementApp button,#managementApp .badge,#managementApp .delta,#managementApp .stat-top span,#managementApp .sync').forEach(el=>{if(el.children.length===0){const key=el.textContent.trim();if(from.has(key))el.textContent=from.get(key)}});
    }
    function renderWorker(){
      const accountMode=portalState.mode==='account'&&portalState.account;
      const list=accountMode?liveJobs().filter(j=>String(j.worker||'').toLowerCase()===portalState.account.name.toLowerCase()):liveJobs();
      const job=list[0];
      const status=job?(job.checkInAt?L('Arrived','Llegó'):/assign/i.test(job.status||'')?L('Assigned','Asignado'):job.status):(accountMode?L(copy.en.accountEmpty,copy.es.accountEmpty):L(copy.en.previewEmpty,copy.es.previewEmpty));
      const jobCards=list.length?list.map(j=>`<article class="job-card"><div class="job-head"><div><h2>${esc(j.address||j.id)}</h2><p>${esc(j.scope||'')} · ${esc(j.client||'')}</p></div><span class="badge blue">${esc(j.status||'')} · ${j.progress||0}%</span></div><div class="job-body"><div class="job-meta"><div class="meta-box"><span>${L('Work order','Orden')}</span><b>${esc(j.id)}</b></div><div class="meta-box"><span>${L('Contact','Contacto')}</span><b>${esc(j.worker||workerIdentity())}</b></div><div class="meta-box"><span>${L('Progress','Progreso')}</span><b>${j.progress||0}%</b></div></div>${j===job?`<div class="task-list"><label class="task"><input type="checkbox" data-progress="25"><div><b>${L('Take before photos','Tomar fotos antes')}</b><p>${L('Exterior and work area','Exterior y área de trabajo')}</p></div></label><label class="task"><input type="checkbox" data-progress="50"><div><b>${L('Confirm scope with management','Confirmar alcance con management')}</b><p>${L('Open the approved instructions','Abrir instrucciones aprobadas')}</p></div></label><label class="task"><input type="checkbox" data-progress="75"><div><b>${L('Complete listed repairs','Completar reparaciones')}</b><p>${L('Report anything outside the scope','Reportar todo lo que esté fuera del alcance')}</p></div></label><label class="task"><input type="checkbox" data-progress="100"><div><b>${L('Upload after photos','Subir fotos finales')}</b><p>${L('Clear photos of each completed item','Fotos claras de cada reparación terminada')}</p></div></label></div>`:''}</div></article>`).join(''):`<article class="job-card"><div class="job-head"><div><h2>${accountMode?L(copy.en.accountEmpty,copy.es.accountEmpty):L(copy.en.previewEmpty,copy.es.previewEmpty)}</h2><p>${accountMode?L(copy.en.accountEmptyCopy,copy.es.accountEmptyCopy):L(copy.en.previewEmptyCopy,copy.es.previewEmptyCopy)}</p></div></div></article>`;
      const greet=accountMode?portalState.account.name:L('Sample','Ejemplo');
      return `<div class="workspace-head"><div><h1>${L('Good morning','Buenos días')}${greet?`, ${esc(greet)}`:''}</h1><p>${accountMode?esc(portalState.account.email):L(copy.en.sampleCopy,copy.es.sampleCopy)}</p></div><div class="workspace-actions"><button class="secondary" data-paction="messages">✉ ${L('Messages','Mensajes')}</button><button class="primary" data-paction="estimate">＋ ${L('Request estimate','Solicitar estimate')}</button></div></div>
      <div class="sync-card ${accountMode?'account-banner':'preview-banner'}"><div><strong>${accountMode?L(copy.en.accountBanner,copy.es.accountBanner):L(copy.en.sampleBanner,copy.es.sampleBanner)}</strong><p>${accountMode?L(copy.en.accountCopy,copy.es.accountCopy):L(copy.en.sampleCopy,copy.es.sampleCopy)}</p></div><code>${esc(accountMode?portalState.account.email:(job?.id||'SAMPLE'))}</code></div>
      <div class="record-strip"><div class="record-cell"><span>${L('Request','Solicitud')}</span><b>${esc(job?.requestId||'—')}</b></div><div class="record-cell"><span>${L('Property','Propiedad')}</span><b>${esc(job?.address||'—')}</b></div><div class="record-cell"><span>${L('Status','Estado')}</span><b>${esc(status)}</b></div><div class="record-cell"><span>${L('Work order','Orden de trabajo')}</span><b>${esc(job?.id||'—')}</b></div><div class="record-cell"><span>${L('Message thread','Conversación')}</span><b>${esc(job?.threadId||'—')}</b></div></div>
      <div class="guide"><div class="guide-number">1</div><div><h2>${L('First estimate? We guide you step by step.','¿Tu primer estimate? Te guiamos paso a paso.')}</h2><p>${L('Choose the property, select the repair and send it to management for approval.','Escoge la propiedad, selecciona la reparación y envíala a management para aprobación.')}</p></div><div class="guide-progress"><span class="on"></span><span></span><span></span></div></div>
      <div class="quick-grid"><button class="quick-action" data-paction="checkin"><i>✓</i><b>${L('Check in','Marcar llegada')}</b><small>${L('Notify Realtor + Management','Avisar Realtor + Management')}</small></button><button class="quick-action" data-paction="access"><i>⌁</i><b>${L('Request access','Solicitar acceso')}</b><small>${L('Code or entry instructions','Código o instrucciones')}</small></button><button class="quick-action" data-paction="photos"><i>▣</i><b>${L('Add photos','Agregar fotos')}</b><small>${L('Before and after','Antes y después')}</small></button><button class="quick-action" data-paction="change"><i>＋</i><b>${L('Request change','Solicitar cambio')}</b><small>${L('Material or extra work','Material o trabajo extra')}</small></button></div>
      <div class="workspace-grid"><div>${jobCards}
      <article class="easy-estimate" id="easyEstimate"><div class="wizard-head"><h2>${L('Guided estimate','Estimate guiado')}</h2><div class="wizard-steps"><span class="${portalState.wizard===1?'active':''}">1</span><span class="${portalState.wizard===2?'active':''}">2</span><span class="${portalState.wizard===3?'active':''}">3</span></div></div><div class="wizard-body">${renderWizard()}</div></article></div>${renderMessageCenter('worker')}
      </div>`;
    }
    function esc(v){return String(v??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]))}
    const wiz={property:null,newProperty:{address:'',client:'',phone:'',notes:''},repairs:[],query:'',notes:'',catalog:null,requests:[]};
    const categoryEs={'Electrical & Lighting':'Electricidad e iluminación','Roofing & Chimney':'Techos y chimenea','Exterior, Siding & Trim':'Exterior, siding y molduras','Gutters & Drainage':'Canaletas y drenaje','Doors, Windows & Garage':'Puertas, ventanas y garage','Plumbing':'Plomería','HVAC & Water Heater':'HVAC y calentador de agua','Interior, Drywall & Painting':'Interior, drywall y pintura','Flooring, Bathroom & Kitchen':'Pisos, baño y cocina','Crawlspace, Foundation & Structure':'Crawlspace, cimientos y estructura','Deck, Fence, Concrete & Outdoor':'Deck, cerca, concreto y exterior','Attic, Safety, Appliances & Cleaning':'Ático, seguridad, electrodomésticos y limpieza'};
    function catLabel(c){return portalState.lang==='es'?(categoryEs[c]||c):c}
    function workerSpecialties(){return portalState.mode==='account'?(portalState.account?.specialties||[]):[]}
    function catalogCategories(){return [...new Set((wiz.catalog||[]).map(r=>r.category).filter(Boolean))]}
    function specialtyChecks(selected){const cats=catalogCategories();return cats.length?cats.map(c=>`<label class="specialty"><input type="checkbox" value="${esc(c)}" ${selected.includes(c)?'checked':''}><span>${esc(catLabel(c))}</span></label>`).join(''):`<p class="signin-help">${L('Loading…','Cargando…')}</p>`}
    function checkedSpecialties(box){return [...(box?.querySelectorAll('input:checked')||[])].map(i=>i.value)}
    function digitsOnly(input){if(input)input.addEventListener('input',()=>{const v=input.value.replace(/\D/g,'').slice(0,10);if(v!==input.value)input.value=v})}
    function busy(btn,on,text){if(!btn)return;if(on){btn.dataset.label=btn.innerHTML;btn.disabled=true;btn.innerHTML=`<span class="spinner"></span>${esc(text||L('Saving…','Guardando…'))}`}else{btn.disabled=false;if(btn.dataset.label)btn.innerHTML=btn.dataset.label}}
    function workerKey(){return portalState.account?.email||('preview:'+workerIdentity())}
    function wizJob(){return wiz.property?.startsWith('job:')?liveJobs().find(j=>'job:'+j.id===wiz.property):null}
    function wizProperty(){const j=wizJob();if(j)return {address:j.address||j.id,client:j.client||''};if(wiz.property==='new')return {address:wiz.newProperty.address.trim(),client:wiz.newProperty.client.trim()};return null}
    function priceRange(r){return r?`$${Number(r.low).toLocaleString('en-US')}–$${Number(r.high).toLocaleString('en-US')}${r.unit&&r.unit!=='each'?' / '+r.unit:''}`:'—'}
    async function loadCatalog(){if(wiz.catalog)return wiz.catalog;try{let res=await fetch('/api/field/catalog');if(!res.ok)res=await fetch('/repair-catalog.json');wiz.catalog=res.ok?await res.json():[]}catch{wiz.catalog=[]}return wiz.catalog}
    async function loadFieldRequests(){try{const res=await fetch('/api/field/requests?worker='+encodeURIComponent(workerKey()));if(res.ok){wiz.requests=await res.json();if(portalState.portal==='worker')renderPortal('worker')}}catch{}}
    function repairMatches(){
      const words=wiz.query.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g,'').split(/\s+/).filter(Boolean);
      const mine=workerSpecialties();
      const list=(wiz.catalog||[]).filter(r=>!mine.length||mine.includes(r.category)).filter(r=>{const hay=[r.service,r.category,r.customer_en,r.customer_es,r.inspector].join(' ').toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g,'');return words.every(w=>hay.includes(w))});
      return words.length?list.slice(0,24):list.slice().sort((a,b)=>(b.tier==='Quick')-(a.tier==='Quick')).slice(0,12);
    }
    function repairListHtml(){
      if(!wiz.catalog)return `<p>${L('Loading the price catalog…','Cargando el catálogo de precios…')}</p>`;
      const rows=repairMatches();
      return rows.length?rows.map(r=>`<button class="choice ${wiz.repairs.some(x=>x.id===r.id)?'selected':''}" data-wiz-repair="${esc(r.id)}"><b>${esc(r.service)}</b><small>${esc(catLabel(r.category))} · ${esc(priceRange(r))}</small></button>`).join(''):`<p>${L('No repair matches. Try other words, e.g. "outlet", "leak", "siding".','No hay coincidencias. Prueba otras palabras, por ejemplo "outlet", "fuga", "siding".')}</p>`;
    }
    function specialtyNote(){const mine=workerSpecialties();return mine.length?`<p class="wiz-hint">${L('Showing services for your specialties:','Mostrando servicios de tus especialidades:')} <b>${esc(mine.map(catLabel).join(', '))}</b></p>`:''}
    function selectedText(){const n=wiz.repairs.length;return n?L(`${n} service(s) selected: `,`${n} servicio(s) seleccionado(s): `)+wiz.repairs.map(r=>r.service).join(', '):L('Choose one or more services.','Escoge uno o varios servicios.')}
    function wizTotal(){return wiz.repairs.reduce((s,r)=>s+(Number(r.price)||0),0)}
    function toggleRepair(id){const i=wiz.repairs.findIndex(x=>x.id===id);if(i>=0){wiz.repairs.splice(i,1);return}const r=(wiz.catalog||[]).find(x=>x.id===id);if(r)wiz.repairs.push({id:r.id,service:r.service,category:r.category,low:r.low,high:r.high,unit:r.unit,price:Math.round((Number(r.low)+Number(r.high))/2)})}
    function requestsHtml(){
      const rows=wiz.requests.slice(0,5);if(!rows.length)return '';
      return `<div class="wiz-requests"><p class="sub">${L('My requests to management','Mis solicitudes a management')}</p>${rows.map(r=>`<div class="summary-row"><span><b>${esc(r.address)}</b><br><small>${r.kind==='new_property'?L('New property','Nueva propiedad'):esc(r.items?.length?r.items.map(i=>`${i.repairName} ${moneyFmt(i.price)}`).join(', '):r.repairName)}${r.clientName?' · '+esc(r.clientName):''}</small></span><span class="badge ${r.status==='Pending'?'amber':r.status==='Declined'?'red':'green'}">${esc(requestStatus(r.status))}</span></div>`).join('')}</div>`;
    }
    function requestStatus(s){const es={Pending:'Pendiente',Reviewed:'Revisada',Approved:'Aprobada',Declined:'Rechazada',Converted:'En management'},en={Converted:'In management'};return portalState.lang==='es'?(es[s]||s):(en[s]||s)
    }
    function renderWizard(){
      if(!wiz.property&&liveJobs()[0])wiz.property='job:'+liveJobs()[0].id;
      if(portalState.wizard===1){
        const list=liveJobs();
        const choices=list.map(j=>`<button class="choice ${wiz.property==='job:'+j.id?'selected':''}" data-wiz-property="job:${esc(j.id)}"><b>${esc(j.address||j.id)}</b><small>${esc(j.scope||j.status||'')}</small></button>`).join('');
        const p=wiz.newProperty;
        const form=wiz.property==='new'?`<div class="new-property-form"><label><span>${L('Property address','Dirección de la propiedad')} *</span><input id="wizAddress" value="${esc(p.address)}" placeholder="${L('Street, city, state','Calle, ciudad, estado')}"></label><label><span>${L('Client or owner','Cliente o propietario')}</span><input id="wizClient" value="${esc(p.client)}" placeholder="${L('Name','Nombre')}"></label><label><span>${L('Client phone','Teléfono del cliente')}</span><input id="wizPhone" value="${esc(p.phone)}" inputmode="numeric" maxlength="10" placeholder="7045550000"></label><label class="full"><span>${L('Notes for management','Notas para management')}</span><textarea id="wizNotesNew" rows="2" placeholder="${L('Access, who referred it, what you saw…','Acceso, quién lo refirió, qué viste…')}">${esc(p.notes)}</textarea></label><div class="full new-property-actions"><button class="secondary" data-wiz-send-property>${L('Only request this property','Solo solicitar esta propiedad')}</button></div></div>`:'';
        return `<p class="sub">${L('Step 1 · Choose the property','Paso 1 · Escoge la propiedad')}</p><div class="choice-list">${choices}<button class="choice ${wiz.property==='new'?'selected':''}" data-wiz-new><b>＋ ${L('New property','Nueva propiedad')}</b><small>${L('Ask management','Solicitar a management')}</small></button></div>${list.length?'':`<p class="wiz-hint">${L('No assigned property yet. Use “New property” to request one.','Aún no hay propiedad asignada. Usa “Nueva propiedad” para solicitarla.')}</p>`}${form}<div style="display:flex;justify-content:flex-end;margin-top:14px"><button class="primary wizard-next">${L('Next: choose repair','Siguiente: escoger reparación')} →</button></div>${requestsHtml()}`;
      }
      if(portalState.wizard===2)return `<p class="sub">${L('Step 2 · Choose what needs repair (you can choose several)','Paso 2 · Escoge qué necesita reparación (puedes escoger varios)')}</p>${specialtyNote()}<input id="wizSearch" class="wiz-search" value="${esc(wiz.query)}" placeholder="${L('Search a repair: outlet, leak, siding…','Busca una reparación: outlet, fuga, siding…')}"><div class="choice-list" id="wizRepairList">${repairListHtml()}</div><p class="wiz-selected" id="wizSelected">${esc(selectedText())}</p><div style="display:flex;justify-content:space-between;margin-top:14px"><button class="secondary wizard-back">← ${L('Back','Atrás')}</button><button class="primary wizard-next">${L('Review','Revisar')} →</button></div>`;
      const prop=wizProperty();
      return `<p class="sub">${L('Step 3 · Review before sending','Paso 3 · Revisa antes de enviar')}</p><div class="summary-box"><div class="summary-row"><span>${L('Property','Propiedad')}</span><b>${esc(prop?.address||L('No property','Sin propiedad'))}${wiz.property==='new'?` · ${L('new','nueva')}`:''}</b></div>${prop?.client?`<div class="summary-row"><span>${L('Client','Cliente')}</span><b>${esc(prop.client)}</b></div>`:''}</div><p class="sub">${L('Final price per service (you can edit it)','Valor final por servicio (lo puedes editar)')}</p><div class="summary-box">${wiz.repairs.map(r=>`<div class="summary-row price-row"><span><b>${esc(r.service)}</b><br><small>${L('Suggested','Sugerido')}: ${esc(priceRange(r))}</small></span><label class="price-input">$<input type="number" min="1" step="1" inputmode="decimal" data-wiz-price="${esc(r.id)}" value="${esc(r.price)}" aria-label="${L('Final price','Valor final')}"></label></div>`).join('')}<div class="summary-row total"><span>${L('Total','Total')}</span><b id="wizTotal">${moneyFmt(wizTotal())}</b></div></div><label class="wiz-notes"><span>${L('Notes for management (optional)','Notas para management (opcional)')}</span><textarea id="wizNotes" rows="2">${esc(wiz.notes)}</textarea></label><div style="display:flex;justify-content:space-between"><button class="secondary wizard-back">← ${L('Back','Atrás')}</button><button class="primary wizard-send">${L('Send to management','Enviar a management')}</button></div>`;
    }
    async function sendWorkerRequest(withRepair){
      const prop=wizProperty();
      if(!prop||prop.address.length<5){toast(L('Write the property address','Escribe la dirección de la propiedad'));return false}
      const p=wiz.newProperty,isNew=wiz.property==='new';
      if(isNew&&p.phone.trim()&&!/^\d{10}$/.test(p.phone.trim())){toast(copy[portalState.lang].badPhone);document.getElementById('wizPhone')?.focus();return false}
      const items=withRepair?wiz.repairs.map(r=>({repairId:r.id,repairName:r.service,price:Number(r.price)})):[];
      if(withRepair&&!items.length){toast(L('Choose at least one service','Escoge al menos un servicio'));return false}
      if(items.some(i=>!(i.price>0))){toast(L('Write a final price above $0 for every service','Escribe un valor final mayor a $0 para cada servicio'));return false}
      const notes=[isNew?p.notes.trim():'',withRepair?wiz.notes.trim():''].filter(Boolean).join('\n');
      try{
        const res=await fetch('/api/field/requests',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({worker:workerKey(),workerName:workerIdentity(),address:prop.address,clientName:prop.client,clientPhone:isNew?p.phone.trim():'',notes,items})});
        const body=await res.json().catch(()=>({}));
        if(!res.ok){toast(body.errorEs&&portalState.lang==='es'?body.errorEs:body.error||L('The request could not be saved','No se pudo guardar la solicitud'));return false}
        wiz.requests=[body,...wiz.requests];
      }catch{toast(L('The request could not be saved','No se pudo guardar la solicitud'));return false}
      const job=wizJob();
      if(job&&withRepair&&window.SafeApi)SafeApi.post('/api/safe/messages',{requestId:job.requestId,senderRole:'Worker',senderName:workerIdentity(),channel:'job',body:L('Estimate request: ','Solicitud de estimate: ')+items.map(i=>`${i.repairName} ${moneyFmt(i.price)}`).join(', '),sourceApp:'Core'}).then(()=>refreshHub(true)).catch(()=>{});
      return true;
    }
    function resetWizard(){portalState.wizard=1;wiz.property=null;wiz.newProperty={address:'',client:'',phone:'',notes:''};wiz.repairs=[];wiz.query='';wiz.notes=''}
    function bindWizard(){
      const rerender=()=>{renderPortal('worker');document.getElementById('easyEstimate')?.scrollIntoView({block:'nearest'})};
      document.querySelectorAll('[data-wiz-property]').forEach(b=>b.onclick=()=>{wiz.property=b.dataset.wizProperty;rerender()});
      document.querySelectorAll('[data-wiz-new]').forEach(b=>b.onclick=()=>{wiz.property='new';rerender();document.getElementById('wizAddress')?.focus()});
      [['wizAddress','address'],['wizClient','client'],['wizPhone','phone'],['wizNotesNew','notes']].forEach(([id,key])=>{const i=document.getElementById(id);if(i)i.oninput=()=>{if(key==='phone')i.value=i.value.replace(/\D/g,'').slice(0,10);wiz.newProperty[key]=i.value}});
      const notes=document.getElementById('wizNotes');if(notes)notes.oninput=()=>{wiz.notes=notes.value};
      document.querySelectorAll('[data-wiz-price]').forEach(i=>i.oninput=()=>{const r=wiz.repairs.find(x=>x.id===i.dataset.wizPrice);if(r)r.price=i.value;const total=document.getElementById('wizTotal');if(total)total.textContent=moneyFmt(wizTotal())});
      document.querySelectorAll('[data-wiz-send-property]').forEach(b=>b.onclick=async()=>{busy(b,true,L('Sending…','Enviando…'));if(await sendWorkerRequest(false)){toast(L('Property request sent to Management','Solicitud de propiedad enviada a Management'));resetWizard();rerender()}else busy(b,false)});
      const search=document.getElementById('wizSearch');
      const bindRepairs=()=>document.querySelectorAll('[data-wiz-repair]').forEach(b=>b.onclick=()=>{toggleRepair(b.dataset.wizRepair);b.classList.toggle('selected',wiz.repairs.some(x=>x.id===b.dataset.wizRepair));const s=document.getElementById('wizSelected');if(s)s.textContent=selectedText()});
      if(search){search.oninput=()=>{wiz.query=search.value;document.getElementById('wizRepairList').innerHTML=repairListHtml();bindRepairs()};if(!wiz.catalog)loadCatalog().then(()=>{const box=document.getElementById('wizRepairList');if(box){box.innerHTML=repairListHtml();bindRepairs()}})}
      bindRepairs();
      document.querySelectorAll('.wizard-next').forEach(b=>b.onclick=()=>{
        if(portalState.wizard===1){if(!wiz.property){toast(L('Choose a property or add a new one','Escoge una propiedad o agrega una nueva'));return}if(wiz.property==='new'&&wiz.newProperty.address.trim().length<5){toast(L('Write the property address','Escribe la dirección de la propiedad'));document.getElementById('wizAddress')?.focus();return}}
        if(portalState.wizard===2&&!wiz.repairs.length){toast(L('Choose at least one service from the catalog','Escoge al menos un servicio del catálogo'));return}
        portalState.wizard=Math.min(3,portalState.wizard+1);rerender()});
      document.querySelectorAll('.wizard-back').forEach(b=>b.onclick=()=>{portalState.wizard=Math.max(1,portalState.wizard-1);rerender()});
      document.querySelectorAll('.wizard-send').forEach(b=>b.onclick=async()=>{busy(b,true,L('Sending…','Enviando…'));if(await sendWorkerRequest(true)){toast(L('Estimate request sent to Management','Solicitud de estimate enviada a Management'));resetWizard();rerender()}else busy(b,false)});
    }
    function renderMessageCenter(type){
      const isEmail=portalState.channel==='email';
      const list=liveMessages();
      const acts=activityBubbles();
      const bubbles=!list.length&&acts?acts:(list.length?list.map(m=>`<div class="bubble ${m.senderRole==='Worker'?'mine':''}">${m.senderRole==='Worker'?'':`<b>${esc(m.senderName)}</b><br>`}${esc(m.body)}<small>${esc(m.senderRole||'')} · ${esc(m.channel||'job')}</small></div>`).join('')+acts:`<div class="bubble"><b>Management</b><br>${L('The assigned Realtor will receive your arrival and access requests here.','El Realtor asignado recibirá aquí tu llegada y solicitudes de acceso.')}<small>${esc(portalState.threadId||'IndorDB')}</small></div>`);
      return `<aside class="message-center" id="messageCenter"><div class="message-tabs"><button class="channel-tab ${!isEmail?'active':''}" data-channel="message">${L('Shared job thread','Conversación del trabajo')}</button><button class="channel-tab ${isEmail?'active':''}" data-channel="email">Management</button></div><div class="messages">${isEmail?privateBubbles():bubbles}</div><div class="composer"><input id="portalComposer" maxlength="2000" placeholder="${isEmail?L('Write privately to Management…','Escribir privado a Management…'):L('Write to Realtor + Management…','Escribir a Realtor + Management…')}"><button id="portalSend" aria-label="${L('Send','Enviar')}">→</button></div></aside>`;
    }
    function whenText(v){return new Date(v).toLocaleString(portalState.lang==='es'?'es-US':'en-US',{dateStyle:'short',timeStyle:'short'})}
    function privateBubbles(){
      const list=fieldLog.items.filter(a=>a.kind==='message'||a.kind==='reply').slice().reverse();
      const intro=`<div class="bubble"><b>Management</b><br>${L('Use this private channel for internal questions, receipts and company instructions. Management answers here.','Usa este canal privado para preguntas internas, recibos e instrucciones de la compañía. Management responde aquí.')}<small>${L('private','privado')}</small></div>`;
      return intro+list.map(a=>a.kind==='reply'?`<div class="bubble"><b>${esc(a.author||'Management')}</b><br>${esc(a.body)}<small>${esc(whenText(a.createdAt))} · Management</small></div>`:`<div class="bubble mine">${esc(a.body)}<small>${esc(whenText(a.createdAt))} · ${L('private','privado')}</small></div>`).join('');
    }
    function errorText(body,fallback){return (portalState.lang==='es'&&body?.errorEs)||body?.error||fallback}
    function refreshMessageCenter(keep){
      const mc=document.getElementById('messageCenter');if(!mc)return;
      const value=keep??document.getElementById('portalComposer')?.value??'',focused=document.activeElement?.id==='portalComposer';
      mc.outerHTML=renderMessageCenter('worker');
      const input=document.getElementById('portalComposer');if(input){input.value=value;if(focused)input.focus()}
      bindMessageCenter();
      const box=document.querySelector('#messageCenter .messages');if(box)box.scrollTop=box.scrollHeight;
    }
    async function sendPortalMessage(){
      const input=document.getElementById('portalComposer'),btn=document.getElementById('portalSend'),message=(input?.value||'').trim();
      if(!message)return;
      if(portalState.channel==='email'){
        busy(btn,true,'…');
        try{
          const res=await fetch('/api/field/activity',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({worker:workerKey(),workerName:workerIdentity(),kind:'message',body:message})});
          const saved=await res.json().catch(()=>({}));
          if(!res.ok){toast(errorText(saved,L('The message could not be sent','No se pudo enviar el mensaje')));busy(btn,false);return}
          fieldLog.items=[saved,...fieldLog.items];refreshMessageCenter('');toast(L('Private message sent to Management','Mensaje privado enviado a Management'));
        }catch{toast(L('The message could not be sent','No se pudo enviar el mensaje'));busy(btn,false)}
        return;
      }
      const requestId=portalState.requestId||liveJobs()[0]?.requestId;
      if(!requestId||!window.SafeApi){toast(L('There is no assigned job yet. Use the Management tab to write privately.','Aún no hay un trabajo asignado. Usa la pestaña Management para escribir en privado.'));return}
      busy(btn,true,'…');
      try{
        await SafeApi.post('/api/safe/messages',{requestId,threadId:portalState.threadId,senderRole:'Worker',senderName:workerIdentity(),channel:'job',body:message,sourceApp:'Core'});
        await refreshHub(false);refreshMessageCenter('');toast(L('Realtor and Management were notified','Realtor y Management fueron notificados'));
      }catch{toast(L('The message could not be sent','No se pudo enviar el mensaje'));busy(btn,false)}
    }
    function bindMessageCenter(){
      document.querySelectorAll('.channel-tab').forEach(b=>b.onclick=()=>{portalState.channel=b.dataset.channel;refreshMessageCenter('')});
      const send=document.getElementById('portalSend');if(send)send.onclick=sendPortalMessage;
      const input=document.getElementById('portalComposer');if(input)input.onkeydown=e=>{if(e.key==='Enter'){e.preventDefault();sendPortalMessage()}};
    }
    function renderRealtor(){
      const rows=(portalState.hub?.contacts||[]).filter(c=>c.role==='Client');
      const props=portalState.hub?.properties||[];
      const clientRows=rows.length?rows.map(c=>{const p=props.find(x=>x.clientId===c.id);return `<tr><td><div class="name">${esc(c.name)}</div><div class="sub">${esc(c.role)}</div></td><td>${esc(p?.address||'—')}</td><td><span class="badge blue">${esc(p?.status||'')}</span></td><td><button class="mini">${L('Open','Abrir')}</button></td></tr>`}).join(''):`<tr><td colspan="4">${L('No clients in IndorDB yet.','Aún no hay clientes en IndorDB.')}</td></tr>`;
      return `<div class="workspace-head"><div><h1>${L('Welcome back','Bienvenido')}</h1><p>${L('Your clients, repair guidance and follow-ups are ready in one place.','Tus clientes, consultas de reparación y seguimientos están en un solo lugar.')}</p></div><div class="workspace-actions"><button class="secondary" data-paction="messages">✉ ${L('Messages','Mensajes')}</button><button class="primary" data-paction="consult">＋ ${L('New consultation','Nueva consulta')}</button></div></div>
      <div class="guide"><div class="guide-number">1</div><div><h2>${L('Need a quick repair answer?','¿Necesitas una respuesta rápida de reparación?')}</h2><p>${L('Upload the inspection report or search the price guide. You can request SAFE review at any step.','Sube el inspection report o busca en la guía de precios. Puedes solicitar revisión de SAFE en cualquier paso.')}</p></div><div class="guide-progress"><span class="on"></span><span></span><span></span></div></div>
      <div class="quick-grid"><button class="quick-action" data-paction="upload"><i>↑</i><b>${L('Inspection report','Inspection report')}</b><small>${L('Upload and organize','Subir y organizar')}</small></button><button class="quick-action" data-paction="price"><i>＄</i><b>${L('Repair prices','Precios de reparación')}</b><small>${L('Fast homeowner guidance','Guía rápida para clientes')}</small></button><button class="quick-action" data-paction="gift"><i>◆</i><b>${L('Create gift card','Crear gift card')}</b><small>${L('After closing','Después del cierre')}</small></button><button class="quick-action" data-paction="followup"><i>✉</i><b>${L('Client follow-up','Seguimiento')}</b><small>${L('Email or message','Correo o mensaje')}</small></button></div>
      <div class="workspace-grid"><article class="job-card"><div class="job-head"><div><h2>${L('Active clients','Clientes activos')}</h2><p>${L('Everything connected to the property','Todo conectado a la propiedad')}</p></div><span class="badge blue">${rows.length} ${L('active','activos')}</span></div><div class="table-card"><table><thead><tr><th>${L('Client','Cliente')}</th><th>${L('Property','Propiedad')}</th><th>${L('Next step','Siguiente paso')}</th><th></th></tr></thead><tbody>${clientRows}</tbody></table></div></article>${renderMessageCenter('realtor')}</div>
      <article class="easy-estimate"><div class="wizard-head"><h2>${L('Quick repair consultation','Consulta rápida de reparación')}</h2><span class="badge green">${L('No login for first use','Primer uso sin login')}</span></div><div class="wizard-body"><div class="choice-list"><button class="choice"><b>${L('Search a repair','Buscar reparación')}</b><small>${L('Use inspector wording','Usar palabras del inspector')}</small></button><button class="choice"><b>${L('Upload report','Subir reporte')}</b><small>PDF · ${L('SAFE review','Revisión SAFE')}</small></button><button class="choice"><b>${L('Ask management','Preguntar a management')}</b><small>${L('Send photos and notes','Enviar fotos y notas')}</small></button></div></div></article>
      `;
    }
    const fieldLog={items:[],photos:[]};
    const actionCopy={
      checkin:{icon:'✓',title:()=>L('Check in at the property','Marcar llegada a la propiedad'),send:()=>L('Register arrival','Registrar llegada'),done:()=>L('Arrival registered and sent to the Realtor and Management','Llegada registrada y enviada al Realtor y Management')},
      access:{icon:'⌁',title:()=>L('Request access','Solicitar acceso'),send:()=>L('Send request','Enviar solicitud'),done:()=>L('Access request sent to Management','Solicitud de acceso enviada a Management')},
      photos:{icon:'▣',title:()=>L('Add photos','Agregar fotos'),send:()=>L('Upload photos','Subir fotos'),done:()=>L('Photos saved to the property record','Fotos guardadas en el expediente de la propiedad')},
      change:{icon:'＋',title:()=>L('Request change','Solicitar cambio'),send:()=>L('Send change request','Enviar solicitud de cambio'),done:()=>L('Change request sent to Management','Solicitud de cambio enviada a Management')}
    };
    function fieldProperties(){
      const seen=new Set(),out=[];
      liveJobs().forEach(j=>{const a=(j.address||'').trim();if(a&&!seen.has(a.toLowerCase())){seen.add(a.toLowerCase());out.push({address:a,workOrderId:j.id,label:`${a} · ${L('assigned','asignada')}`})}});
      wiz.requests.forEach(r=>{const a=(r.address||'').trim();if(a&&!seen.has(a.toLowerCase())){seen.add(a.toLowerCase());out.push({address:a,workOrderId:'',label:`${a} · ${L('requested','solicitada')}`})}});
      fieldLog.items.forEach(r=>{const a=(r.address||'').trim();if(a&&!seen.has(a.toLowerCase())){seen.add(a.toLowerCase());out.push({address:a,workOrderId:r.workOrderId||'',label:a})}});
      return out;
    }
    function shrinkPhoto(file){return new Promise((resolve,reject)=>{const reader=new FileReader();reader.onerror=()=>reject(new Error('read'));reader.onload=()=>{const img=new Image();img.onerror=()=>reject(new Error('image'));img.onload=()=>{const max=1280,s=Math.min(1,max/Math.max(img.width,img.height)),c=document.createElement('canvas');c.width=Math.max(1,Math.round(img.width*s));c.height=Math.max(1,Math.round(img.height*s));c.getContext('2d').drawImage(img,0,0,c.width,c.height);resolve(c.toDataURL('image/jpeg',.8))};img.src=reader.result};reader.readAsDataURL(file)})}
    function ensureActionModal(){
      let m=document.getElementById('fieldActionModal');if(m)return m;
      m=document.createElement('div');m.className='modal-backdrop';m.id='fieldActionModal';m.setAttribute('role','dialog');m.setAttribute('aria-modal','true');
      m.addEventListener('click',e=>{if(e.target===m)m.classList.remove('open')});
      document.body.appendChild(m);return m;
    }
    function localStamp(d){const p=n=>String(n).padStart(2,'0');return `${d.getFullYear()}-${p(d.getMonth()+1)}-${p(d.getDate())}T${p(d.getHours())}:${p(d.getMinutes())}`}
    function accessMessage(option){return {
      lockbox:L('I arrived. Please send the lockbox code.','Ya llegué. Por favor envíen el código del lockbox.'),
      gate:L('I arrived. Please send the gate or garage code.','Ya llegué. Por favor envíen el código de la reja o del garage.'),
      owner:L('I arrived. Please ask the owner or tenant to open the property.','Ya llegué. Por favor pidan al dueño o inquilino que abra la propiedad.'),
      other:L('I arrived. Please send the instructions to enter the property.','Ya llegué. Por favor envíen las instrucciones para entrar a la propiedad.')
    }[option]||''}
    function openFieldAction(kind){
      const m=ensureActionModal(),copy=actionCopy[kind],props=fieldProperties(),now=new Date();
      fieldLog.photos=[];
      const propField=`<div class="field full"><label>${L('Property','Propiedad')} *</label>${props.length?`<select id="faProperty">${props.map((p,i)=>`<option value="${i}">${esc(p.label)}</option>`).join('')}<option value="other">${L('Other address…','Otra dirección…')}</option></select>`:''}<input id="faAddress" placeholder="${L('Street, city, state','Calle, ciudad, estado')}" ${props.length?'hidden':''}></div>`;
      const extra={
        checkin:`<div class="field full"><label>${L('Arrival time','Hora de llegada')} *</label><input type="datetime-local" id="faArrival" value="${localStamp(now)}" max="${localStamp(now)}"></div><div class="field full"><label>${L('Note (optional)','Nota (opcional)')}</label><textarea id="faBody" placeholder="${L('e.g. Parked in front, owner not home','Ej.: Estacioné enfrente, el dueño no está')}"></textarea></div>`,
        access:`<div class="field full"><label>${L('What do you need?','¿Qué necesitas?')}</label><select id="faDetail"><option value="lockbox">${L('Lockbox code','Código del lockbox')}</option><option value="gate">${L('Gate or garage code','Código de reja o garage')}</option><option value="owner">${L('Owner or tenant to open','Que el dueño o inquilino abra')}</option><option value="other">${L('Other instructions','Otras instrucciones')}</option></select></div><div class="field full"><label>${L('Message for Management','Mensaje para Management')} *</label><textarea id="faBody">${esc(accessMessage('lockbox'))}</textarea></div>`,
        photos:`<div class="field"><label>${L('Photo type','Tipo de fotos')}</label><select id="faDetail"><option value="before">${L('Before','Antes')}</option><option value="during">${L('During','Durante')}</option><option value="after">${L('After','Después')}</option></select></div><div class="field"><label>${L('Photos (up to 8)','Fotos (hasta 8)')} *</label><label class="fa-file"><input type="file" id="faPhotos" accept="image/*" multiple><span id="faPhotosLabel">${L('Choose or take photos','Elegir o tomar fotos')}</span></label></div><div class="field full"><div class="fa-thumbs" id="faThumbs"></div></div><div class="field full"><label>${L('Description (optional)','Descripción (opcional)')}</label><textarea id="faBody" placeholder="${L('What the photos show','Qué muestran las fotos')}"></textarea></div>`,
        change:`<div class="field"><label>${L('Change type','Tipo de cambio')}</label><select id="faDetail"><option value="material">${L('Material','Material')}</option><option value="extra">${L('Extra work','Trabajo extra')}</option><option value="scope">${L('Scope change','Cambio de alcance')}</option></select></div><div class="field"><label>${L('Estimated cost (optional)','Costo estimado (opcional)')}</label><input id="faCost" inputmode="decimal" placeholder="$0"></div><div class="field full"><label>${L('Describe the change','Describe el cambio')} *</label><textarea id="faBody" placeholder="${L('What is needed and why','Qué se necesita y por qué')}"></textarea></div>`
      }[kind];
      m.innerHTML=`<div class="modal"><div class="modal-head"><h2>${copy.icon} ${esc(copy.title())}</h2><button class="close" data-fa-close>×</button></div><div class="modal-body"><div class="form-grid">${propField}${extra}</div></div><div class="modal-foot"><button class="secondary" data-fa-close>${L('Cancel','Cancelar')}</button><button class="primary" id="faSend">${esc(copy.send())}</button></div></div>`;
      m.querySelectorAll('[data-fa-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
      const sel=document.getElementById('faProperty'),addr=document.getElementById('faAddress');
      if(sel)sel.onchange=()=>{addr.hidden=sel.value!=='other';if(!addr.hidden)addr.focus()};
      const detail=document.getElementById('faDetail'),body=document.getElementById('faBody');
      if(kind==='access'&&detail&&body)detail.onchange=()=>{const fresh=accessMessage(detail.value);if(!body.value.trim()||body.dataset.auto!=='0')body.value=fresh;body.dataset.auto='1'};
      if(kind==='access'&&body)body.oninput=()=>{body.dataset.auto='0'};
      const photos=document.getElementById('faPhotos');
      if(photos)photos.onchange=async()=>{
        const files=[...photos.files].filter(f=>/^image\//.test(f.type));
        if(fieldLog.photos.length+files.length>8)toast(L('Up to 8 photos per upload','Hasta 8 fotos por envío'));
        const label=document.getElementById('faPhotosLabel'),sendBtn=document.getElementById('faSend');
        if(label)label.innerHTML=`<span class="spinner"></span>${esc(L('Preparing photos…','Preparando fotos…'))}`;
        if(sendBtn)sendBtn.disabled=true;
        for(const f of files.slice(0,8-fieldLog.photos.length)){try{fieldLog.photos.push(await shrinkPhoto(f))}catch{toast(L('One photo could not be read','No se pudo leer una foto'))}}
        if(sendBtn)sendBtn.disabled=false;
        photos.value='';renderThumbs();
      };
      document.getElementById('faSend').onclick=()=>submitFieldAction(kind,props);
      m.classList.add('open');
    }
    function renderThumbs(){
      const box=document.getElementById('faThumbs');if(!box)return;
      box.innerHTML=fieldLog.photos.map((p,i)=>`<span class="fa-thumb"><img src="${p}" alt=""><button type="button" data-fa-remove="${i}" aria-label="${L('Remove','Quitar')}">×</button></span>`).join('');
      box.querySelectorAll('[data-fa-remove]').forEach(b=>b.onclick=()=>{fieldLog.photos.splice(Number(b.dataset.faRemove),1);renderThumbs()});
      const label=document.getElementById('faPhotosLabel');if(label)label.textContent=fieldLog.photos.length?L(`${fieldLog.photos.length} photo(s) ready · add more`,`${fieldLog.photos.length} foto(s) lista(s) · agregar más`):L('Choose or take photos','Elegir o tomar fotos');
    }
    async function submitFieldAction(kind,props){
      const sel=document.getElementById('faProperty'),addr=document.getElementById('faAddress');
      const picked=sel&&sel.value!=='other'?props[Number(sel.value)]:{address:(addr?.value||'').trim(),workOrderId:''};
      if(!picked.address||picked.address.length<5){toast(L('Write the property address','Escribe la dirección de la propiedad'));addr?.focus();return}
      const body=(document.getElementById('faBody')?.value||'').trim();
      if((kind==='access'||kind==='change')&&!body){toast(L('Write the message for Management','Escribe el mensaje para Management'));document.getElementById('faBody').focus();return}
      if(kind==='photos'&&!fieldLog.photos.length){toast(L('Add at least one photo','Agrega al menos una foto'));return}
      let detail=document.getElementById('faDetail')?.value||'';
      const cost=(document.getElementById('faCost')?.value||'').trim();if(kind==='change'&&cost)detail+=` · ${cost.startsWith('$')?cost:'$'+cost}`;
      if(kind==='checkin'){
        const arrival=new Date(document.getElementById('faArrival')?.value||'');
        if(isNaN(arrival)){toast(L('Write the arrival time','Escribe la hora de llegada'));return}
        if(arrival.getTime()>Date.now()+60000){toast(L("The arrival time can't be in the future","La hora de llegada no puede estar en el futuro"));return}
        detail=arrival.toISOString();
      }
      const btn=document.getElementById('faSend');busy(btn,true,kind==='photos'?L(`Uploading ${fieldLog.photos.length} photo(s)…`,`Subiendo ${fieldLog.photos.length} foto(s)…`):L('Saving…','Guardando…'));
      try{
        const res=await fetch('/api/field/activity',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({worker:workerKey(),workerName:workerIdentity(),kind,workOrderId:picked.workOrderId||'',address:picked.address,detail,body,photos:kind==='photos'?fieldLog.photos:[]})});
        const saved=await res.json().catch(()=>({}));
        if(!res.ok){toast(errorText(saved,L('It could not be saved','No se pudo guardar')));busy(btn,false);return}
        fieldLog.items=[saved,...fieldLog.items];
      }catch{toast(L('It could not be saved','No se pudo guardar'));busy(btn,false);return}
      if(kind==='checkin'&&picked.workOrderId&&window.SafeApi)SafeApi.post('/api/safe/work-orders/'+encodeURIComponent(picked.workOrderId)+'/check-in',{}).then(()=>refreshHub(true)).catch(()=>{});
      document.getElementById('fieldActionModal').classList.remove('open');fieldLog.photos=[];
      portalState.channel='message';renderPortal('worker');toast(actionCopy[kind].done());
    }
    function activityText(a){
      const detailEs={lockbox:'código del lockbox',gate:'código de reja o garage',owner:'que abran el dueño o inquilino',other:'otras instrucciones',before:'antes',during:'durante',after:'después',material:'material',extra:'trabajo extra',scope:'cambio de alcance'};
      const d=String(a.detail||''),dk=d.split(' · ')[0],dl=portalState.lang==='es'?(detailEs[dk]||dk)+d.slice(dk.length):d;
      if(a.kind==='checkin'){const at=new Date(a.detail);return `✓ ${L('Arrived at','Llegada a')} ${a.address}${isNaN(at)?'':' · '+at.toLocaleString(portalState.lang==='es'?'es-US':'en-US',{dateStyle:'short',timeStyle:'short'})}${a.body?' — '+a.body:''}`}
      if(a.kind==='access')return `⌁ ${L('Access request','Solicitud de acceso')} (${dl}): ${a.body}`;
      if(a.kind==='photos')return `▣ ${a.photos.length} ${L('photo(s)','foto(s)')} · ${dl} · ${a.address}${a.body?' — '+a.body:''}`;
      return `＋ ${L('Change requested','Cambio solicitado')} (${dl}): ${a.body}`;
    }
    function activityBubbles(){
      return fieldLog.items.filter(a=>a.kind!=='message'&&a.kind!=='reply').reverse().map(a=>`<div class="bubble mine">${esc(activityText(a))}${a.photos?.length?`<div class="fa-bubble-photos">${a.photos.slice(0,4).map(p=>`<img src="${p}" alt="">`).join('')}</div>`:''}<small>${esc(new Date(a.createdAt).toLocaleString(portalState.lang==='es'?'es-US':'en-US',{dateStyle:'short',timeStyle:'short'}))} · ${L('saved','guardado')}</small></div>`).join('');
    }
    function arrivalOf(a){const at=new Date(a.detail);return isNaN(at)?new Date(a.createdAt):at}
    function todayCheckin(){const d=new Date().toDateString();return fieldLog.items.find(a=>a.kind==='checkin'&&arrivalOf(a).toDateString()===d)}
    async function loadFieldActivity(poll){try{const res=await fetch('/api/field/activity?worker='+encodeURIComponent(workerKey()));if(!res.ok)return;const items=await res.json();const changed=items.map(a=>a.id).join()!==fieldLog.items.map(a=>a.id).join();fieldLog.items=items;if(portalState.portal!=='worker')return;if(!poll)renderPortal('worker');else if(changed)refreshMessageCenter()}catch{}}
    function renderPortal(portal){
      portalState.portal='worker';document.getElementById('portalRole').textContent=L('Worker Workspace','Portal del trabajador');document.getElementById('portalAvatar').textContent=(workerIdentity()||'W')[0].toUpperCase();document.getElementById('workspaceContent').innerHTML=renderWorker();bindPortalActions();
    }
    function bindPortalActions(){
      document.querySelectorAll('[data-paction]').forEach(b=>b.onclick=()=>{const a=b.dataset.paction;if(a==='estimate'){document.getElementById('easyEstimate')?.scrollIntoView({behavior:'smooth'})}else if(a==='messages'){document.getElementById('messageCenter')?.scrollIntoView({behavior:'smooth'})}else if(['checkin','access','photos','change'].includes(a)){openFieldAction(a)}});
      bindMessageCenter();
      document.querySelectorAll('.task input').forEach(input=>{const job=liveJobs()[0];if(job&&Number(job.progress||0)>=Number(input.dataset.progress||0))input.checked=true;input.onchange=async()=>{const values=[...document.querySelectorAll('.task input')];const done=values.filter(x=>x.checked).length;const pct=Math.round((done/values.length)*100);const wo=portalState.workOrderId||job?.id;if(wo&&window.SafeApi){try{await SafeApi.patch('/api/safe/work-orders/'+encodeURIComponent(wo),{progress:pct,status:pct>=100?'completed':'in_progress'});await refreshHub(false)}catch{}}toast(L(`Progress sent to Management · ${pct}%`,`Progreso enviado a Management · ${pct}%`))}});if(liveJobs()[0]?.checkInAt){const checkin=document.querySelector('[data-paction="checkin"] b');if(checkin)checkin.textContent=L('Checked in','Llegada registrada')}
      const arrived=todayCheckin();if(arrived){const card=document.querySelector('[data-paction="checkin"]');if(card){card.classList.add('done');card.querySelector('b').textContent=L('Checked in','Llegada registrada');card.querySelector('small').textContent=`${arrivalOf(arrived).toLocaleTimeString(portalState.lang==='es'?'es-US':'en-US',{hour:'numeric',minute:'2-digit'})} · ${arrived.address}`}}
      bindWizard();
    }
    function openPortal(mode){
      portalState.mode=mode==='account'?'account':'preview';
      document.getElementById('gateway').classList.add('portal-hidden');document.getElementById('managementApp').classList.add('portal-hidden');document.getElementById('portalWorkspace').classList.add('portal-hidden');
      portalState.portal='worker';location.hash=portalState.mode==='account'?'account':'preview';
      updateProfileButton();
      document.getElementById('portalWorkspace').classList.remove('portal-hidden');resetWizard();wiz.requests=[];fieldLog.items=[];renderPortal('worker');loadFieldRequests();loadFieldActivity();
      window.scrollTo(0,0);
    }
    function showGate(mode){
      const choices=document.getElementById('gateChoices'),create=document.getElementById('gateCreate'),sign=document.getElementById('gateSignIn'),title=document.getElementById('gateTitle');
      if(!choices)return;
      choices.hidden=mode!=='choices';create.hidden=mode!=='create';sign.hidden=mode!=='signin';
      create.querySelectorAll('input,button').forEach(el=>{if(el.type!=='button')el.disabled=mode!=='create'});
      sign.querySelectorAll('input,button').forEach(el=>{if(el.type!=='button')el.disabled=mode!=='signin'});
      const key=mode==='create'?'createProfile':mode==='signin'?'signInExisting':'gateTitle';
      title.dataset.i18n=key;title.textContent=copy[portalState.lang][key];
    }
    function fieldAccounts(){try{return JSON.parse(localStorage.getItem('safe-field-accounts')||'[]')}catch{return[]}}
    function forgetLegacyAccount(email){const rest=fieldAccounts().filter(a=>a.email.toLowerCase()!==email.toLowerCase());if(rest.length)localStorage.setItem('safe-field-accounts',JSON.stringify(rest));else localStorage.removeItem('safe-field-accounts')}
    function rememberAccount(account){const a={name:account.name,email:account.email,company:account.company||'',phone:account.phone||'',specialties:account.specialties||[]};portalState.account=a;portalState.mode='account';sessionStorage.setItem('safe-field-session',JSON.stringify(a))}
    function updateProfileButton(){
      const b=document.getElementById('createFromPortal');if(!b)return;
      const acct=portalState.mode==='account'&&portalState.account;
      b.dataset.i18n=acct?'myProfile':'createProfile';b.textContent=copy[portalState.lang][b.dataset.i18n];
      b.onclick=acct?openProfileModal:()=>{showGateway();showGate('create')};
    }
    function renderCreateSpecialties(){const box=document.getElementById('createSpecialties');if(box)box.innerHTML=specialtyChecks(checkedSpecialties(box))}
    function openProfileModal(){
      const a=portalState.account||{},m=ensureActionModal(),c=copy[portalState.lang];
      m.innerHTML=`<div class="modal"><div class="modal-head"><h2>${esc(c.myProfile)}</h2><button class="close" data-fa-close>×</button></div><div class="modal-body"><div class="form-grid">
        <div class="field"><label>${esc(c.fullName)} *</label><input id="pfName" value="${esc(a.name)}"></div>
        <div class="field"><label>${esc(c.workEmail)}</label><input value="${esc(a.email)}" disabled></div>
        <div class="field"><label>${esc(c.companyName)}</label><input id="pfCompany" maxlength="200" value="${esc(a.company)}"></div>
        <div class="field"><label>${esc(c.phone10)}</label><input id="pfPhone" inputmode="numeric" maxlength="10" value="${esc(a.phone)}"></div>
        <div class="field full"><label>${esc(c.specialties)} *</label><div class="specialty-grid" id="pfSpecialties">${specialtyChecks(a.specialties||[])}</div></div>
      </div></div><div class="modal-foot"><button class="secondary" data-fa-close>${L('Cancel','Cancelar')}</button><button class="primary" id="pfSave">${L('Save profile','Guardar perfil')}</button></div></div>`;
      m.querySelectorAll('[data-fa-close]').forEach(b=>b.onclick=()=>m.classList.remove('open'));
      digitsOnly(document.getElementById('pfPhone'));
      if(!wiz.catalog)loadCatalog().then(()=>{const box=document.getElementById('pfSpecialties');if(box)box.innerHTML=specialtyChecks(a.specialties||[])});
      document.getElementById('pfSave').onclick=async()=>{
        const btn=document.getElementById('pfSave'),name=document.getElementById('pfName').value.trim(),company=document.getElementById('pfCompany').value.trim(),phone=document.getElementById('pfPhone').value.trim(),specialties=checkedSpecialties(document.getElementById('pfSpecialties'));
        if(name.length<2){toast(c.fullName);return}
        if(phone&&!/^\d{10}$/.test(phone)){toast(c.badPhone);return}
        if(!specialties.length){toast(c.chooseSpecialty);return}
        busy(btn,true);
        try{
          const res=await fetch('/api/field/profiles/'+encodeURIComponent(a.email),{method:'PUT',headers:{'Content-Type':'application/json'},body:JSON.stringify({name,company,phone,specialties})});
          const body=await res.json().catch(()=>({}));
          if(res.status===404){toast(L('Sign in again to move your account to the database.','Vuelve a ingresar para pasar tu cuenta a la base de datos.'));busy(btn,false);return}
          if(!res.ok){toast(errorText(body,c.saveFailed));busy(btn,false);return}
          rememberAccount(body);m.classList.remove('open');renderPortal('worker');toast(c.profileSaved);
        }catch{toast(c.saveFailed);busy(btn,false)}
      };
      m.classList.add('open');
    }
    function showGateway(){portalState.portal=null;portalState.mode=null;portalState.account=null;sessionStorage.removeItem('safe-field-session');history.replaceState(null,'',location.pathname);showGate('choices');document.getElementById('gateway').classList.remove('portal-hidden');document.getElementById('managementApp').classList.add('portal-hidden');document.getElementById('portalWorkspace').classList.add('portal-hidden');window.scrollTo(0,0)}
    const sidebar=document.getElementById('sidebar');
    function showView(name){
      document.querySelectorAll('.view').forEach(v=>v.classList.remove('active'));
      document.getElementById('view-'+name).classList.add('active');
      document.querySelectorAll('.nav button').forEach(b=>b.classList.toggle('active',b.dataset.view===name));
      document.getElementById('crumbTitle').textContent=titles[name]||'SAFE Core';
      sidebar.classList.remove('open');document.getElementById('mobileOverlay').classList.remove('open');window.scrollTo({top:0,behavior:'smooth'});
    }
    document.querySelectorAll('[data-view]').forEach(b=>b.addEventListener('click',()=>showView(b.dataset.view)));
    document.querySelectorAll('[data-go]').forEach(a=>a.addEventListener('click',e=>{e.preventDefault();showView(a.dataset.go)}));
    document.getElementById('mobileMenu').onclick=()=>{sidebar.classList.add('open');document.getElementById('mobileOverlay').classList.add('open')};
    document.getElementById('mobileOverlay').onclick=()=>{sidebar.classList.remove('open');document.getElementById('mobileOverlay').classList.remove('open')};
    function openModal(id){document.getElementById(id).classList.add('open')}
    function closeModal(id){document.getElementById(id).classList.remove('open')}
    document.querySelectorAll('.open-estimate').forEach(b=>b.onclick=()=>openModal('estimateModal'));
    document.getElementById('quickEstimate').onclick=()=>openModal('estimateModal');
    document.querySelectorAll('.open-client').forEach(b=>b.onclick=()=>openModal('clientModal'));
    document.getElementById('addClientTop').onclick=()=>openModal('clientModal');
    document.querySelectorAll('[data-close]').forEach(b=>b.onclick=()=>closeModal(b.dataset.close));
    document.querySelectorAll('.modal-backdrop').forEach(m=>m.addEventListener('click',e=>{if(e.target===m)closeModal(m.id)}));
    function toast(text){const t=document.getElementById('toast');t.textContent=text;t.classList.add('show');setTimeout(()=>t.classList.remove('show'),2600)}
    document.querySelectorAll('.language-option').forEach(b=>b.addEventListener('click',()=>applyLanguage(b.dataset.lang)));
    function validEmail(value){return /^[^\s@]+@[^\s@]+\.[A-Za-z]{2,}$/.test(String(value||'').trim())}
    function rejectBadEmail(input){const value=(input?.value||'').trim();if(validEmail(value)){input?.setCustomValidity('');return true}const msg=copy[portalState.lang].badEmail;if(input){input.setCustomValidity(msg);input.reportValidity();input.focus()}else toast(msg);return false}
    ['createEmail','signInEmail'].forEach(id=>document.getElementById(id)?.addEventListener('input',e=>e.target.setCustomValidity('')));
    document.getElementById('showCreate').onclick=()=>showGate('create');
    document.getElementById('showSignIn').onclick=()=>showGate('signin');
    document.getElementById('createToSignIn').onclick=()=>showGate('signin');
    document.getElementById('signInToCreate').onclick=()=>showGate('create');
    document.querySelectorAll('.portal-choice').forEach(b=>b.addEventListener('click',()=>{portalState.account=null;portalState.mode='preview';openPortal('preview')}));
    document.querySelectorAll('.portal-home').forEach(b=>b.addEventListener('click',showGateway));
    document.getElementById('signinForm').addEventListener('submit',async e=>{
      e.preventDefault();
      const creating=!document.getElementById('gateCreate').hidden;
      if(creating){
        const name=document.getElementById('profileName').value.trim();
        const emailInput=document.getElementById('createEmail');
        const password=document.getElementById('createPassword').value;
        if(!name){toast(copy[portalState.lang].fullName);return}
        if(!rejectBadEmail(emailInput))return;
        if(password.length<6){toast(copy[portalState.lang].shortPassword);return}
        const company=document.getElementById('createCompany').value.trim(),phone=document.getElementById('createPhone').value.trim(),specialties=checkedSpecialties(document.getElementById('createSpecialties'));
        if(phone&&!/^\d{10}$/.test(phone)){toast(copy[portalState.lang].badPhone);document.getElementById('createPhone').focus();return}
        if(!specialties.length){toast(copy[portalState.lang].chooseSpecialty);return}
        const email=emailInput.value.trim(),btn=e.submitter;
        busy(btn,true);
        try{
          const res=await fetch('/api/field/profiles',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({email,name,company,phone,specialties,password})});
          const body=await res.json().catch(()=>({}));
          if(res.status===409){toast(copy[portalState.lang].accountExists);showGate('signin');document.getElementById('signInEmail').value=email;return}
          if(!res.ok){toast(errorText(body,copy[portalState.lang].saveFailed));return}
          forgetLegacyAccount(email);
          if(window.SafeApi)SafeApi.post('/api/safe/clients',{name,email,phone,firm:company,role:'Worker'}).catch(()=>{});
          rememberAccount(body);openPortal('account');toast(copy[portalState.lang].profileReady);
        }catch{toast(copy[portalState.lang].saveFailed)}
        finally{busy(btn,false)}
        return;
      }
      const emailInput=document.getElementById('signInEmail');
      const password=document.getElementById('signInPassword').value;
      if(!rejectBadEmail(emailInput))return;
      const email=emailInput.value.trim(),btn=e.submitter;
      busy(btn,true);
      try{
        const res=await fetch('/api/field/profiles/signin',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({email,password})});
        const body=await res.json().catch(()=>({}));
        if(res.ok){rememberAccount(body);openPortal('account');toast(copy[portalState.lang].signedIn);return}
        if(res.status===404){
          const legacy=fieldAccounts().find(a=>a.email.toLowerCase()===email.toLowerCase()&&a.password===password);
          if(legacy){showGate('create');document.getElementById('profileName').value=legacy.name;document.getElementById('createEmail').value=legacy.email;document.getElementById('createPassword').value=password;toast(copy[portalState.lang].completeProfile);return}
        }
        toast(res.status===404||res.status===401?copy[portalState.lang].noAccount:errorText(body,copy[portalState.lang].saveFailed));
      }catch{toast(copy[portalState.lang].saveFailed)}
      finally{busy(btn,false)}
    });
    digitsOnly(document.getElementById('createPhone'));
    loadCatalog().then(renderCreateSpecialties);
    document.querySelectorAll('.portal-home [data-i18n]').forEach(node=>node.dataset.i18n='allPortals');
    const portalBar=document.querySelector('.portal-bar-right');
    if(portalBar&&!document.getElementById('createFromPortal')){
      const create=document.createElement('button');
      create.type='button';create.id='createFromPortal';create.className='secondary';create.dataset.i18n='createProfile';
      create.onclick=()=>{showGateway();showGate('create')};
      portalBar.prepend(create);
    }
    applyLanguage('en');
    let savedAccount=null;
    try{savedAccount=JSON.parse(sessionStorage.getItem('safe-field-session')||'null')}catch{}
    if(savedAccount?.email&&savedAccount?.name){rememberAccount(savedAccount);openPortal('account')}
    else showGateway();
    fillManagement();
    refreshHub(false);
    setInterval(()=>{refreshHub(portalState.portal==='worker');if(portalState.portal==='worker')loadFieldActivity(true)},12000);
    document.getElementById('saveClient').onclick=async()=>{
      const n=document.getElementById('newClientName').value.trim();
      const clientEmail=(document.getElementById('newClientEmail')?.value||'').trim();
      if(!n){toast('Nombre requerido');return}
      if(clientEmail&&!validEmail(clientEmail)){toast(copy[portalState.lang].badEmail);return}
      try{
        await SafeApi.post('/api/safe/clients',{name:n,role:'Client',phone:document.getElementById('newClientPhone')?.value,email:document.getElementById('newClientEmail')?.value,address:document.getElementById('newClientAddress')?.value});
        await refreshHub(false);closeModal('clientModal');toast(n+' guardado en IndorDB');showView('clients');
      }catch{toast('No se pudo guardar en IndorDB')}
    };
    document.getElementById('saveEstimate').onclick=async()=>{
      const client=document.getElementById('estClient')?.value||'';
      const address=document.getElementById('estProperty')?.value||'';
      const scope=document.getElementById('estProjectName')?.value||'Estimate';
      let total=0;document.querySelectorAll('.estimate-line').forEach(l=>{const sel=l.querySelector('.line-service'),q=Number(l.querySelector('.line-qty')?.value)||0;total+=(Number(sel?.value)||0)*q});
      if(!address&&!client){toast('Agrega un cliente o propiedad primero');return}
      try{
        await SafeApi.post('/api/safe/requests',{address:address||'TBD',client,contact:client||'Management',type:scope,sourceApp:'Core',source:'Management',items:document.querySelectorAll('.estimate-line').length,value:total});
        await refreshHub(false);closeModal('estimateModal');toast('Estimate guardado en IndorDB');showView('estimates');
      }catch{toast('No se pudo guardar en IndorDB')}
    };
    function recalc(){let total=0;document.querySelectorAll('.estimate-line').forEach(l=>{const sel=l.querySelector('.line-service'),q=Number(l.querySelector('.line-qty').value)||0;const value=Number(sel.value)||0;total+=value*q;l.querySelector('.line-price').value=new Intl.NumberFormat('en-US',{style:'currency',currency:'USD'}).format(value*q)});document.getElementById('estimateTotal').textContent=new Intl.NumberFormat('en-US',{style:'currency',currency:'USD'}).format(total)}
    document.getElementById('addLine').onclick=()=>{const line=document.createElement('div');line.className='estimate-line';line.innerHTML='<select class="line-service"><option value="100">Visita diagnóstica</option><option value="3500">Crawlspace makeover</option><option value="2500">Retoque de pintura</option><option value="8500">Remodelación baño básica</option></select><input class="line-qty" type="number" min="1" value="1" aria-label="Cantidad"><input class="line-price" value="$100.00" aria-label="Precio"><button class="remove-line" aria-label="Eliminar">×</button>';document.getElementById('estimateLines').appendChild(line);bindLine(line);recalc()};
    function bindLine(line){line.querySelectorAll('select,input').forEach(x=>x.addEventListener('input',recalc));line.querySelector('.remove-line').onclick=()=>{line.remove();recalc()}}
    document.querySelectorAll('.estimate-line').forEach(bindLine);
    const drawer=document.getElementById('estimateDrawer'),cover=document.getElementById('drawerCover');
    function openDrawer(){drawer.classList.add('open');cover.classList.add('open')}
    function closeDrawer(){drawer.classList.remove('open');cover.classList.remove('open')}
    document.querySelectorAll('.open-detail').forEach(b=>b.onclick=openDrawer);document.getElementById('closeDrawer').onclick=closeDrawer;cover.onclick=closeDrawer;
    document.getElementById('convertInvoice').onclick=()=>{closeDrawer();showView('invoices');toast('Invoice creado desde el estimate aprobado')};
    document.getElementById('makeWorkOrder').onclick=()=>{closeDrawer();showView('team');toast('Work order creado y listo para asignar')};
    document.querySelectorAll('.approve').forEach(b=>b.onclick=()=>{b.textContent='Aprobado ✓';b.disabled=true;toast('Aprobación registrada')});
    document.getElementById('clientSearch').addEventListener('input',e=>{const q=e.target.value.toLowerCase();document.querySelectorAll('#clientsBody tr').forEach(r=>r.style.display=r.textContent.toLowerCase().includes(q)?'':'none')});
    function filterPrices(){const q=document.getElementById('priceSearch').value.toLowerCase(),trade=document.getElementById('tradeFilter').value;document.querySelectorAll('#priceBody tr').forEach(r=>{const okText=r.textContent.toLowerCase().includes(q),okTrade=!trade||r.dataset.trade===trade;r.style.display=okText&&okTrade?'':'none'})}
    document.getElementById('priceSearch').addEventListener('input',filterPrices);document.getElementById('tradeFilter').addEventListener('change',filterPrices);
    document.getElementById('dictatePrice').onclick=()=>toast('Vista previa: aquí se dictará costo, labor y precio vendido');
    document.getElementById('globalSearch').addEventListener('keydown',e=>{if(e.key==='Enter'){showView('clients');document.getElementById('clientSearch').value=e.target.value;document.getElementById('clientSearch').dispatchEvent(new Event('input'));toast('Resultados para “'+e.target.value+'”')}});
    document.querySelectorAll('.role-pill').forEach(p=>p.onclick=()=>{document.querySelectorAll('.role-pill').forEach(x=>x.classList.remove('active'));p.classList.add('active');toast('Vista de permisos: '+p.textContent)});
    const modelContext=document.modelContext;
    if(modelContext?.registerTool){
      const register=(tool)=>{try{Promise.resolve(modelContext.registerTool(tool)).catch(()=>{})}catch(_){}}
      register({name:'navigate_safe_core',title:'Abrir sección de SAFE Core',description:'Abre una sección visible del sistema de management.',inputSchema:{type:'object',properties:{section:{type:'string',enum:Object.keys(titles)}},required:['section'],additionalProperties:false},annotations:{readOnlyHint:true,untrustedContentHint:false},execute({section}){if(!titles[section])throw new Error('Sección no válida');showView(section);return{section,title:titles[section]}}});
      register({name:'start_safe_estimate',title:'Comenzar estimate',description:'Abre el formulario visible para crear un nuevo estimate.',inputSchema:{type:'object',properties:{},additionalProperties:false},annotations:{readOnlyHint:false,untrustedContentHint:false},execute(){openModal('estimateModal');return{status:'form_open'}}});
      register({name:'search_safe_prices',title:'Buscar precios',description:'Busca servicios en el catálogo maestro y muestra los resultados visibles.',inputSchema:{type:'object',properties:{query:{type:'string'}},required:['query'],additionalProperties:false},annotations:{readOnlyHint:true,untrustedContentHint:false},execute({query}){if(typeof query!=='string')throw new Error('Consulta no válida');showView('prices');document.getElementById('priceSearch').value=query;filterPrices();return{query,status:'filtered'}}});
    }