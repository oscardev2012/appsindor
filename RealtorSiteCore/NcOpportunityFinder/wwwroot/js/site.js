    const copy={en:{scan:'Oscar algorithm import active · North Carolina',flow:'Imported property flow',all:'Third-party posts',info:'Needs information',compare:'Ready to compare',offer:'Offer review',queue:'Work queue',roadmap:'Roadmap & integration',map:'NC market map',contact:'Seller contact',visits:'Site visits',how:'What this platform does',howText:'Oscar’s existing algorithm brings the Facebook property posts. This platform receives, organizes and analyzes them; it does not search MLS or Realtor listings.',north:'All North Carolina⌄',propertyType:'Third-party sale posts⌄',testBox:'Test buy box',property:'Property',asking:'Asking',information:'Information',next:'Next step',score:'Score',insight:'<b>Input is already found.</b> Oscar’s algorithm brings the third-party Facebook posts; this platform turns them into a clear review and offer workflow.',hint:'Click a property to open its analysis.',roadmapLabel:'Roadmap & integration',roadmapTitle:'Connect Oscar’s existing algorithm first',roadmapIntro:'The Facebook property-search logic already exists. The MVP begins when Oscar’s algorithm imports a property post into this platform through an API/webhook.',p1t:'Import',p1d:'Oscar’s existing algorithm sends the property post, source link, date, images and extracted information.',p2t:'Validate',p2d:'Create/update the property record, preserve the source and flag the exact data still missing.',p3t:'Enrich',p3d:'Deduplicate, find comparable sales, estimate ARV and create repair ranges.',p4t:'Underwrite',p4d:'Use the 65% rule to produce a maximum offer and explain the calculation.',p5t:'Human action',p5d:'A team member chooses: request data, contact, visit, offer or discard.',buildNow:'Build now · MVP',now1:'Property record that receives the existing algorithm output: source, post URL, date, images, text, asking price, area and seller signals.',now2:'Protected API/webhook: Oscar Algorithm → Opportunity Finder property import.',now3:'Deduplication + queue: Imported posts → Needs information → Ready to compare → Offer review.',now4:'ARV, repairs, 65% offer calculator and full audit history.',connect:'Exact connection',connect1:'Oscar Algorithm sends a normalized property event; it does not need to search Facebook again.',connect2:'Platform returns property/lead ID, duplicate result and missing fields.',connect3:'Analysis, score and offer are saved in the same property record.',connect4:'When a seller replies, automation pauses and a human takes over.'},es:{scan:'Importación del algoritmo de Óscar activa · North Carolina',flow:'Flujo de propiedades importadas',all:'Posts de terceros',info:'Necesita información',compare:'Lista para comparar',offer:'Revisión de oferta',queue:'Cola de trabajo',roadmap:'Roadmap e integración',map:'Mapa de NC',contact:'Contacto al vendedor',visits:'Visitas a la propiedad',how:'Qué hace esta plataforma',howText:'El algoritmo existente de Óscar trae los posts de casas desde Facebook. Esta plataforma los recibe, organiza y analiza; no busca MLS ni listings de Realtors.',north:'Todo North Carolina⌄',propertyType:'Posts de venta por terceros⌄',testBox:'Probar buy box',property:'Propiedad',asking:'Precio pedido',information:'Información',next:'Siguiente paso',score:'Score',insight:'<b>La información ya viene encontrada.</b> El algoritmo de Óscar trae los posts de terceros en Facebook; esta plataforma los convierte en una cola clara de revisión y oferta.',hint:'Haz clic en una propiedad para abrir el análisis.',roadmapLabel:'Roadmap e integración',roadmapTitle:'Primero conectar el algoritmo existente de Óscar',roadmapIntro:'La lógica para encontrar casas en Facebook ya existe. El MVP comienza cuando el algoritmo de Óscar importa un post de propiedad a esta plataforma por API/webhook.',p1t:'Importar',p1d:'El algoritmo existente de Óscar envía el post de propiedad, enlace fuente, fecha, imágenes e información extraída.',p2t:'Validar',p2d:'Crear o actualizar el registro de propiedad, conservar la fuente y marcar exactamente qué información falta.',p3t:'Enriquecer',p3d:'Eliminar duplicados, buscar comparables, estimar ARV y crear rangos de reparación.',p4t:'Análisis de compra',p4d:'Usar la regla de 65% para calcular la oferta máxima y explicar el cálculo.',p5t:'Acción humana',p5d:'Una persona elige: pedir datos, contactar, visitar, ofertar o descartar.',buildNow:'Construir ahora · MVP',now1:'Registro de propiedad que recibe la salida del algoritmo existente: fuente, URL del post, fecha, imágenes, texto, precio pedido, área y señales del vendedor.',now2:'API/webhook protegido: Algoritmo de Óscar → Importación de propiedad a Opportunity Finder.',now3:'Deduplicación + cola: Posts importados → Necesita información → Lista para comparar → Revisión de oferta.',now4:'ARV, repairs, calculadora 65% e historial completo de cambios.',connect:'Conexión exacta',connect1:'El algoritmo de Óscar manda un evento de propiedad normalizado; no tiene que buscar Facebook otra vez.',connect2:'La plataforma devuelve property/lead ID, resultado de duplicado y campos faltantes.',connect3:'El análisis, score y oferta se guardan en el mismo registro de propiedad.',connect4:'Cuando responde el vendedor, la automatización se pausa y toma control una persona.'}};
    Object.assign(copy.en,{calcLabel:'Buy box calculator',calcTitle:'Calculate a logical offer',calcIntro:'The system uses your 65% rule to protect the total project margin.',calcRepairs:'Repairs',calcClosing:'Closing + holding',calcContingency:'Contingency',calcResult:'Maximum offer',calcFormula:'ARV × 65% − repairs − closing/holding − contingency',calcSave:'Save numbers to this property'});
    Object.assign(copy.es,{calcLabel:'Calculadora de buy box',calcTitle:'Calcular una oferta lógica',calcIntro:'El sistema usa tu regla de 65% para proteger el margen total del proyecto.',calcRepairs:'Repairs',calcClosing:'Cierre + holding',calcContingency:'Contingencia',calcResult:'Oferta máxima',calcFormula:'ARV × 65% − repairs − cierre/holding − contingencia',calcSave:'Guardar números en esta propiedad'});
    const ui={
      en:{views:{all:['Third-party for-sale property posts','Imported from Oscar’s existing algorithm. These are owner and third-party Facebook posts—not Realtor listings or MLS.','Newest third-party property posts',['Imported today','Seller signals','Complete enough for comps','Fit the 65% buy box']],info:['Needs information','Strong seller signals, but the team needs key facts before running a complete comparison.','Leads waiting for seller information',['Needs a follow-up','Missing address','Missing photos','Missing price']],compare:['Ready to compare','These homes have enough verified information for a preliminary ARV, repair range, and maximum offer.','Properties ready for numbers review',['Ready for comps','Average comparables','Repairs estimate needed','Highest score']],offer:['Offer review','These properties have a preliminary offer number. A person decides what happens next.','Deals awaiting a human decision',['With offer number','Awaiting review','In human review','Fit the 65% buy box']]},
        missing:{address:'address',price:'price',photos:'photos'},missingPrefix:'Missing: ',complete:'Complete',comps:n=>`${n} comparable sales`,maxOfferShort:'max offer',notPosted:'Not posted',pending:'Pending',pendingData:'Pending data',fit:{green:'Green',yellow:'Yellow'},
        next:{requestPhotos:'Request photos',contactSeller:'Contact seller',runComps:'Run comps',estimateRepairs:'Estimate repairs',prepareOffer:'Prepare offer',inReview:'In human review'},
        whyInfo:m=>`Do not discard. Ask the seller for: ${m}.`,whyCompare:'Information is complete. Run comps and estimate repairs to get the maximum offer.',whyFits:'The asking price fits the 65% buy box.',whyAbove:d=>`Asking is ${d} above our maximum offer.`,
        loading:'Loading properties…',empty:'No properties imported yet. They appear here when Oscar’s algorithm sends them to /api/opportunity/import.',emptyView:'No properties in this stage.',emptyDetail:'Select a property to see its analysis.',
        lastImport:d=>` · last import ${d}`,noImports:' · no imports yet',viewPost:'Open original post',photos:n=>`${n} photo${n===1?'':'s'}`,posted:'Posted',postFrom:'Published on',copyLink:'Copy link',linkCopied:'Post link copied',noLink:'The source did not send the post link.',fullPost:'Original post',showMore:'Show full post',showLess:'Show less',noPhotos:'The post has no photos.',photoNote:'Photos are hosted by the source and can expire. If one does not load, open the original post.',
        history:'History',historyEmpty:'No changes yet.',events:{imported:e=>`Imported from ${e.source||'source'}`,updated:()=>'Updated by a new import',numbers:e=>`Offer numbers saved · max offer ${money(e.maxOffer)}`,review:()=>'Marked for human review'},
        reviewToast:'Lead marked for human review.',reviewDone:'Already in human review',savedToast:'Numbers saved.',loadFailed:'Could not load properties.'},
      es:{views:{all:['Publicaciones de casas a la venta por terceros','Importadas por el algoritmo existente de Óscar. Son posts de dueños y terceros en Facebook—no listings de Realtors ni MLS.','Publicaciones más recientes de propiedades por terceros',['Importadas hoy','Señales de vendedor','Listas para comparables','Cumplen el buy box 65%']],info:['Necesita información','Estas casas tienen señales fuertes, pero faltan datos para analizar los números.','Leads esperando información del vendedor',['Necesitan seguimiento','Falta dirección','Faltan fotos','Falta precio']],compare:['Listas para comparar','Estas casas tienen información suficiente para un ARV, repairs y oferta preliminar.','Propiedades listas para revisión de números',['Listas para comps','Comparables promedio','Falta estimar repairs','Score más alto']],offer:['Revisión de oferta','Estas propiedades ya tienen una oferta preliminar. Una persona decide el siguiente paso.','Deals esperando decisión humana',['Con oferta calculada','Esperando revisión','En revisión humana','Cumplen el buy box 65%']]},
        missing:{address:'dirección',price:'precio',photos:'fotos'},missingPrefix:'Falta: ',complete:'Completa',comps:n=>`${n} ventas comparables`,maxOfferShort:'oferta máx.',notPosted:'Sin precio',pending:'Pendiente',pendingData:'Datos pendientes',fit:{green:'Verde',yellow:'Amarillo'},
        next:{requestPhotos:'Pedir fotos',contactSeller:'Contactar vendedor',runComps:'Buscar comparables',estimateRepairs:'Estimar repairs',prepareOffer:'Preparar oferta',inReview:'En revisión humana'},
        whyInfo:m=>`No descartar. Pedir al vendedor: ${m}.`,whyCompare:'La información está completa. Busca comparables y estima repairs para obtener la oferta máxima.',whyFits:'El precio pedido cabe en el buy box de 65%.',whyAbove:d=>`El precio pedido está ${d} arriba de nuestra oferta máxima.`,
        loading:'Cargando propiedades…',empty:'Todavía no hay propiedades importadas. Aparecen aquí cuando el algoritmo de Óscar las envía a /api/opportunity/import.',emptyView:'No hay propiedades en esta etapa.',emptyDetail:'Selecciona una propiedad para ver su análisis.',
        lastImport:d=>` · última importación ${d}`,noImports:' · sin importaciones todavía',viewPost:'Ver post original',photos:n=>`${n} foto${n===1?'':'s'}`,posted:'Publicado',postFrom:'Publicado en',copyLink:'Copiar link',linkCopied:'Link del post copiado',noLink:'La fuente no envió el link del post.',fullPost:'Post original',showMore:'Ver post completo',showLess:'Ver menos',noPhotos:'El post no tiene fotos.',photoNote:'Las fotos están alojadas en la fuente y pueden caducar. Si alguna no carga, abre el post original.',
        history:'Historial',historyEmpty:'Sin cambios todavía.',events:{imported:e=>`Importada desde ${e.source||'la fuente'}`,updated:()=>'Actualizada por una nueva importación',numbers:e=>`Números de oferta guardados · oferta máxima ${money(e.maxOffer)}`,review:()=>'Marcada para revisión humana'},
        reviewToast:'Lead marcado para revisión humana.',reviewDone:'Ya está en revisión humana',savedToast:'Números guardados.',loadFailed:'No se pudieron cargar las propiedades.'}
    };
    Object.assign(copy.en,{scan:'Apify search · any US city',scanButton:'Search listings',scanLabel:'Apify · every source',scanTitle:'Who is selling a house in this city?',scanIntro:'Pick a US city. Apify searches every selected network at the same time and saves each house with price, phone, email, seller profile and photos. The same house found in two places is merged and scored so you can compare.',scanState:'State',scanCity:'City',scanSources:'Sources',scanAdvanced:'Source settings for this city',scanSlug:'Marketplace city code',scanSite:'Craigslist area',scanSlugHint:'Filled from the city name. If a source returns nothing, open facebook.com/marketplace or craigslist.org, pick the city and copy the code from the address bar.',scanGroups:'Facebook groups (one link per line)',scanLimitLabel:'Results per source',scanRun:'Search every source now',scanHistory:'Recent searches',
      map:'US market map',howText:'Apify searches Facebook Marketplace and groups, Craigslist, Zillow by owner, X, Reddit, Instagram and TikTok for any US city. This platform merges, scores and compares what it finds; agent posts are left out.',
      insight:'<b>Every source, one list.</b> The same house posted on two sites becomes one lead with all its contacts, so you compare price, discount and seller signals side by side.',
      roadmapTitle:'How a city search becomes an offer',roadmapIntro:'Apify finds every owner posting a house for sale in the city you pick. The platform keeps the evidence, fills what is missing, scores each house and asks a person before any outreach or offer.',
      p1d:'Apify searches each selected network for the chosen city and saves post link, date, photos, text, phone and email.',now2:'Search panel: pick state and city, choose sources, see the cost before running and the progress of every source.',connect1:'Each source runs as its own Apify actor at the same time; a failure in one does not stop the others.'});
    Object.assign(copy.es,{scan:'Búsqueda Apify · cualquier ciudad de EE.UU.',scanButton:'Buscar anuncios',scanLabel:'Apify · todas las fuentes',scanTitle:'¿Quién vende casa en esta ciudad?',scanIntro:'Elige una ciudad de EE.UU. Apify busca en todas las redes seleccionadas al mismo tiempo y guarda cada casa con precio, teléfono, correo, perfil del vendedor y fotos. La misma casa encontrada en dos lugares se une y recibe un score para comparar.',scanState:'Estado',scanCity:'Ciudad',scanSources:'Fuentes',scanAdvanced:'Ajustes de fuentes para esta ciudad',scanSlug:'Código de ciudad en Marketplace',scanSite:'Zona de Craigslist',scanSlugHint:'Se llena con el nombre de la ciudad. Si una fuente no trae nada, abre facebook.com/marketplace o craigslist.org, elige la ciudad y copia el código de la barra de direcciones.',scanGroups:'Grupos de Facebook (un enlace por línea)',scanLimitLabel:'Resultados por fuente',scanRun:'Buscar en todas las fuentes',scanHistory:'Búsquedas recientes',
      map:'Mapa de mercado EE.UU.',howText:'Apify busca en Facebook Marketplace y grupos, Craigslist, Zillow dueño directo, X, Reddit, Instagram y TikTok en cualquier ciudad de EE.UU. Esta plataforma une, califica y compara lo que encuentra; los posts de agentes se descartan.',
      insight:'<b>Todas las fuentes, una sola lista.</b> La misma casa publicada en dos sitios queda como un solo lead con todos sus contactos, para comparar precio, descuento y señales del vendedor lado a lado.',
      roadmapTitle:'Cómo una búsqueda por ciudad termina en oferta',roadmapIntro:'Apify encuentra a cada dueño que publica una casa a la venta en la ciudad que elijas. La plataforma guarda la evidencia, completa lo que falta, califica cada casa y pide una decisión humana antes de contactar u ofertar.',
      p1d:'Apify busca en cada red seleccionada para la ciudad elegida y guarda link del post, fecha, fotos, texto, teléfono y correo.',now2:'Panel de búsqueda: elegir estado y ciudad, escoger fuentes, ver el costo antes de correr y el avance de cada fuente.',connect1:'Cada fuente corre como su propio actor de Apify al mismo tiempo; si una falla, las demás siguen.'});
    const extra={
      en:{allDescription:'Houses for sale found by Apify on Facebook Marketplace and groups, Craigslist, Zillow (by owner), X, Reddit, Instagram and TikTok. Owners first—agent posts are left out.',allTitle:'Houses for sale from every source',
        empty:'No properties yet. Press “Search listings”, pick a city and Apify will bring every house for sale it finds.',
        allCities:'All cities',allStates:'All states',pickStateFirst:'City: pick a state first',allCitiesN:n=>`All cities (${n.toLocaleString('en-US')})`,allSources:'All sources',sort:{score:'Best score',newest:'Newest',discount:'Biggest discount',cheapest:'Lowest price'},
        contact:'Seller contact',seller:'Seller',profile:'Profile',phone:'Phone',email:'Email',noContact:'No phone or email in the post. Message the seller from the original post.',seenIn:'Found in',marketValue:'Market value',discount:'Discount',scoreWhy:'Score',scoreAuto:'calculated',scoreManual:'from the importer',sqft:'sq ft',
        reasons:{discount:d=>d>=0?`${d}% below market value`:`${-d}% above market value`,belowMaxOffer:()=>'Asking fits the 65% buy box',signals:n=>`${n} motivated-seller signal${n==1?'':'s'}`,contact:()=>'Has phone or email',complete:()=>'Address, price and photos present',sources:n=>`Seen in ${n} sources`},
        scanNoToken:'Apify token is not configured yet. Add Apify:ApiToken to the site settings to run searches.',scanPickCity:'Pick a city from the list.',scanNoDb:'The database is not available, so searches cannot be saved yet (run the SQL script and check the connection string).',scanChooseCity:n=>`Choose a city (${n.toLocaleString('en-US')})`,scanPickSource:'Pick at least one source.',
        scanEstimate:(usd,max)=>`Up to ${max.toLocaleString('en-US')} results · about $${usd} on Apify`,scanStarted:'Search started. Results appear as each source finishes.',scanRunning:'running',scanNone:'No searches yet.',needsGroups:'needs group links',perK:'/1k',
        status:{Pending:'Waiting',Running:'Running',Succeeded:'Done',Partial:'Partly done',Failed:'Failed'},
        sourceLine:s=>`${s.found} found · ${s.imported} new · ${s.updated} updated · ${s.skipped} skipped`,searchLine:s=>`${s.found} found · ${s.imported} new · ${s.updated} updated`},
      es:{allDescription:'Casas a la venta encontradas por Apify en Facebook Marketplace y grupos, Craigslist, Zillow (dueño directo), X, Reddit, Instagram y TikTok. Primero dueños—los posts de agentes se descartan.',allTitle:'Casas a la venta de todas las fuentes',
        empty:'Todavía no hay propiedades. Presiona “Buscar anuncios”, elige una ciudad y Apify traerá todas las casas a la venta que encuentre.',
        allCities:'Todas las ciudades',allStates:'Todos los estados',pickStateFirst:'Ciudad: elige primero un estado',allCitiesN:n=>`Todas las ciudades (${n.toLocaleString('en-US')})`,allSources:'Todas las fuentes',sort:{score:'Mejor score',newest:'Más recientes',discount:'Mayor descuento',cheapest:'Precio más bajo'},
        contact:'Contacto del vendedor',seller:'Vendedor',profile:'Perfil',phone:'Teléfono',email:'Correo',noContact:'El post no trae teléfono ni correo. Escríbele al vendedor desde el post original.',seenIn:'Encontrada en',marketValue:'Valor de mercado',discount:'Descuento',scoreWhy:'Score',scoreAuto:'calculado',scoreManual:'del importador',sqft:'pies²',
        reasons:{discount:d=>d>=0?`${d}% debajo del valor de mercado`:`${-d}% arriba del valor de mercado`,belowMaxOffer:()=>'El precio cabe en el buy box 65%',signals:n=>`${n} señal${n==1?'':'es'} de vendedor motivado`,contact:()=>'Tiene teléfono o correo',complete:()=>'Tiene dirección, precio y fotos',sources:n=>`Vista en ${n} fuentes`},
        scanNoToken:'El token de Apify aún no está configurado. Agrega Apify:ApiToken en los ajustes del sitio para buscar.',scanPickCity:'Elige una ciudad de la lista.',scanNoDb:'La base de datos no está disponible, así que todavía no se pueden guardar búsquedas (corre el script SQL y revisa la cadena de conexión).',scanChooseCity:n=>`Elige una ciudad (${n.toLocaleString('en-US')})`,scanPickSource:'Elige al menos una fuente.',
        scanEstimate:(usd,max)=>`Hasta ${max.toLocaleString('en-US')} resultados · cerca de $${usd} en Apify`,scanStarted:'Búsqueda iniciada. Los resultados aparecen a medida que termina cada fuente.',scanRunning:'en curso',scanNone:'Todavía no hay búsquedas.',needsGroups:'requiere enlaces de grupos',perK:'/1k',
        status:{Pending:'En espera',Running:'Buscando',Succeeded:'Lista',Partial:'Parcial',Failed:'Falló'},
        sourceLine:s=>`${s.found} encontrados · ${s.imported} nuevos · ${s.updated} actualizados · ${s.skipped} descartados`,searchLine:s=>`${s.found} encontrados · ${s.imported} nuevos · ${s.updated} actualizados`}
    };
    ['en','es'].forEach(l=>{Object.assign(ui[l],extra[l]);ui[l].views.all[0]=extra[l].allTitle;ui[l].views.all[1]=extra[l].allDescription;ui[l].views.all[2]=extra[l].allTitle});
    let lang='en', current='all', selected=null, properties=[], loaded=false, loadError=null, calcTarget=null, fState='', fCity='', fSource='', sortBy='score';
    const rows=document.getElementById('rows'), detail=document.getElementById('detail'), views=['all','info','compare','offer'];
    const t=()=>ui[lang];
    const money=v=>v==null?'—':new Intl.NumberFormat('en-US',{style:'currency',currency:'USD',maximumFractionDigits:0}).format(v);
    const short=v=>v==null?'—':v>=1000?`$${Math.round(v/1000)}k`:money(v);
    const count=v=>typeof v==='number'?v.toLocaleString('en-US'):v;
    const esc=v=>String(v??'').replace(/[&<>"']/g,ch=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[ch]));
    const when=iso=>iso?new Date(iso).toLocaleString(lang==='es'?'es-MX':'en-US',{dateStyle:'medium',timeStyle:'short'}):'';
    const inView=(p,v)=>v==='all'||p.stage===v;
    const sourcesOf=p=>(p.sources||p.source||'').split(' · ').filter(Boolean);
    const base=()=>properties.filter(p=>(!fState||p.state===fState)&&(!fCity||(p.city||'').toLowerCase()===fCity.toLowerCase())&&(!fSource||sourcesOf(p).includes(fSource)));
    const sorters={score:(a,b)=>(b.score??-1)-(a.score??-1),newest:(a,b)=>(b.postedAt||b.importedAt).localeCompare(a.postedAt||a.importedAt),discount:(a,b)=>(b.discount??-999)-(a.discount??-999),cheapest:(a,b)=>(a.asking??Infinity)-(b.asking??Infinity)};
    function fillSelect(el,options,value){const html=options.map(([v,l])=>`<option value="${esc(v)}">${esc(l)}</option>`).join('');if(el.dataset.html!==html){el.innerHTML=html;el.dataset.html=html}el.value=value}
    function renderFilters(){
      const c=t(), tally=(list,key)=>{const m=new Map();list.forEach(p=>key(p).forEach(k=>m.set(k,(m.get(k)||0)+1)));return[...m].sort((a,b)=>b[1]-a[1])};
      const sources=tally(properties,sourcesOf);
      if(fSource&&!sources.some(([k])=>k===fSource))fSource='';
      fillSelect(document.getElementById('filterSource'),[['',c.allSources],...sources.map(([k,n])=>[k,`${k} (${n})`])],fSource);
      fillSelect(document.getElementById('sortBy'),Object.entries(c.sort),sortBy);
    }
    const apiError=b=>(lang==='es'&&b&&b.errorEs)||(b&&b.error)||t().loadFailed;
    function shortUrl(u){try{const x=new URL(u);const s=(x.hostname.replace(/^www\./,'')+x.pathname).replace(/\/$/,'');return s.length>48?s.slice(0,47)+'…':s}catch{return u}}
    function toast(text){const el=document.getElementById('toast');el.textContent=text;el.classList.add('show');clearTimeout(toast.timer);toast.timer=setTimeout(()=>el.classList.remove('show'),2400)}
    function replace(updated){properties=properties.map(p=>p.id===updated.id?updated:p)}
    async function load(){try{const r=await fetch('/api/opportunity/properties',{cache:'no-store'});const body=await r.json().catch(()=>({}));if(!r.ok)throw body;properties=body;loadError=null}catch(e){properties=[];loadError=e&&(e.error||e.errorEs)?e:{}}loaded=true;render()}
    function applyLanguage(){const c=copy[lang];document.documentElement.lang=lang;document.querySelectorAll('[data-copy]').forEach(el=>el.textContent=c[el.dataset.copy]||el.textContent);document.getElementById('insightText').innerHTML=c.insight;document.getElementById('boardHint').textContent=c.hint;document.getElementById('langToggle').textContent=lang==='en'?'ES':'EN';render()}
    function metrics(view){
      if(!loaded||loadError)return['–','–','–','–'];
      const list=base().filter(p=>inView(p,view)), fits=list.filter(p=>p.fit==='green').length;
      if(view==='all'){const today=new Date().toDateString();return[list.filter(p=>new Date(p.importedAt).toDateString()===today).length,list.filter(p=>p.sellerSignal).length,list.filter(p=>p.stage!=='info').length,fits]}
      if(view==='info')return[list.length,list.filter(p=>p.missing.includes('address')).length,list.filter(p=>p.missing.includes('photos')).length,list.filter(p=>p.missing.includes('price')).length];
      if(view==='compare'){const comps=list.map(p=>p.comparables).filter(n=>n!=null),scores=list.map(p=>p.score).filter(n=>n!=null);return[list.length,comps.length?Math.round(comps.reduce((a,b)=>a+b,0)/comps.length):'–',list.filter(p=>p.repairs==null).length,scores.length?Math.max(...scores):'–']}
      return[list.length,list.filter(p=>p.status==='Active').length,list.filter(p=>p.status==='HumanReview').length,fits];
    }
    const place=p=>[p.city&&`${p.city}, ${p.state}`,p.sources||p.source,p.discount>0?`−${p.discount}%`:'',(p.sellerPhone||p.sellerEmail)?'☎':'',p.sellerSignal].filter(Boolean).join(' · ');
    function infoText(p){
      const c=t();
      if(p.stage==='info')return c.missingPrefix+p.missing.map(m=>c.missing[m]).join(', ');
      if(p.stage==='offer')return`ARV ${short(p.arv)} · repairs ${short(p.repairs)} · ${c.maxOfferShort} ${short(p.maxOffer)}`;
      const parts=[];if(p.comparables!=null)parts.push(c.comps(p.comparables));
      if(p.arvLow!=null&&p.arvHigh!=null)parts.push(`ARV ${short(p.arvLow)}–${short(p.arvHigh)}`);else if(p.arv!=null)parts.push(`ARV ${short(p.arv)}`);
      if(p.repairsLow!=null&&p.repairsHigh!=null)parts.push(`repairs ${short(p.repairsLow)}–${short(p.repairsHigh)}`);
      return parts.join(' · ')||c.complete;
    }
    function nextStep(p){const n=t().next;if(p.status==='HumanReview')return n.inReview;if(p.stage==='info')return p.missing.length===1&&p.missing[0]==='photos'?n.requestPhotos:n.contactSeller;if(p.stage==='compare')return p.arv==null?n.runComps:n.estimateRepairs;return n.prepareOffer}
    function why(p){const c=t();if(p.notes)return p.notes;if(p.stage==='info')return c.whyInfo(p.missing.map(m=>c.missing[m]).join(', '));if(p.stage==='compare')return c.whyCompare;return p.asking<=p.maxOffer?c.whyFits:c.whyAbove(money(p.asking-p.maxOffer))}
    function render(){
      const c=t(), v=c.views[current];
      document.getElementById('pageTitle').textContent=v[0];document.getElementById('pageDescription').textContent=v[1];document.getElementById('boardTitle').textContent=v[2];
      metrics(current).forEach((val,i)=>{document.getElementById('m'+(i+1)).textContent=count(val);document.getElementById('l'+(i+1)).textContent=v[3][i]});
      renderFilters();
      const filtered=base();
      views.forEach(k=>document.getElementById('count-'+k).textContent=loaded&&!loadError?count(filtered.filter(p=>inView(p,k)).length):'–');
      const last=properties.reduce((a,p)=>!a||p.importedAt>a?p.importedAt:a,'');
      document.getElementById('lastImport').textContent=!loaded||loadError?'':last?c.lastImport(when(last)):c.noImports;
      const list=filtered.filter(p=>inView(p,current)).sort(sorters[sortBy]);
      if(!list.some(p=>p.id===selected))selected=list.length?list[0].id:null;
      const message=!loaded?c.loading:loadError?apiError(loadError):!properties.length?c.empty:!list.length?c.emptyView:'';
      rows.innerHTML=message?`<tr><td colspan="5" class="empty${loadError?' error':''}">${esc(message)}</td></tr>`:list.map(p=>`<tr class="data ${p.id===selected?'selected':''}" data-id="${p.id}"><td class="property">${p.images&&/^https?:\/\//i.test(p.images[0])?`<img class="thumb" src="${esc(p.images[0])}" alt="" loading="lazy" referrerpolicy="no-referrer" onerror="this.remove()">`:''}<b>${esc(p.title)}</b><span>${esc(place(p))}</span>${/^https?:\/\//i.test(p.sourceUrl)?`<a class="row-link" href="${esc(p.sourceUrl)}" target="_blank" rel="noopener">↗ ${esc(shortUrl(p.sourceUrl))}</a>`:''}</td><td class="money">${p.asking==null?esc(c.notPosted):money(p.asking)}</td><td>${esc(infoText(p))}</td><td>${esc(nextStep(p))}</td><td><span class="status ${p.fit==='green'?'green':'amber'}"><i></i>${p.score??'—'}</span></td></tr>`).join('');
      rows.querySelectorAll('tr.data').forEach(r=>r.addEventListener('click',e=>{if(e.target.closest('a'))return;selected=Number(r.dataset.id);render()}));
      renderDetail();
    }
    function renderDetail(){
      const c=t(), es=lang==='es', p=properties.find(x=>x.id===selected);
      if(!p){detail.innerHTML=`<div class="detail-label">${es?'Análisis de propiedad':'Property analysis'}</div><p class="muted-note">${loaded&&properties.length?esc(c.emptyDetail):''}</p>`;return}
      const range=(est,low,high)=>est!=null?money(est):low!=null&&high!=null?`${short(low)}–${short(high)}`:c.pending;
      const location=[p.address,[p.city,p.state].filter(Boolean).join(', ')+(p.zip?' '+p.zip:'')].filter(Boolean).join(' · ');
      const step=p.stage==='info'?0:p.stage==='compare'?1:2;
      const link=/^https?:\/\//i.test(p.sourceUrl)?p.sourceUrl:'';
      const origin=`<div class="origin"><div class="origin-top"><div><span>${esc(c.postFrom)}</span><b>${esc(p.postType||p.source)}</b>${p.postedAt?`<small>${esc(c.posted)} ${esc(when(p.postedAt))}</small>`:''}</div>${link?`<a class="origin-link" href="${esc(link)}" target="_blank" rel="noopener">${esc(c.viewPost)} ↗</a>`:''}</div>${link?`<div class="origin-url"><a href="${esc(link)}" target="_blank" rel="noopener">${esc(link)}</a><button type="button" id="copyLink">${esc(c.copyLink)}</button></div>`:`<small class="muted-note">${esc(c.noLink)}</small>`}</div>`;
      const imgs=(p.images||[]).filter(u=>/^https?:\/\//i.test(u));
      const img=(u,lazy)=>`<img src="${esc(u)}" alt="" referrerpolicy="no-referrer"${lazy?' loading="lazy"':''} onerror="this.parentElement.classList.add('broken')">`;
      const gallery=imgs.length?`<div class="gallery"><a class="hero" id="heroPhoto" href="${esc(imgs[0])}" target="_blank" rel="noopener">${img(imgs[0])}</a>${imgs.length>1?`<div class="thumbs">${imgs.map((u,i)=>`<a href="${esc(u)}" data-photo="${i}" class="${i?'':'on'}">${img(u,true)}</a>`).join('')}</div>`:''}<small class="muted-note">${esc(c.photos(imgs.length))} · ${esc(c.photoNote)}</small></div>`:`<p class="muted-note">${esc(c.noPhotos)}</p>`;
      const post=p.postText?`<div class="divider2"></div><div class="detail-label">${esc(c.fullPost)}</div><div class="post-text${p.postText.length>320?' collapsed':''}" id="postText">${esc(p.postText)}</div>${p.postText.length>320?`<button class="linkbutton" id="togglePost">${esc(c.showMore)}</button>`:''}`:'';
      const inReview=p.status==='HumanReview';
      const split=v=>(v||'').split(',').map(x=>x.trim()).filter(Boolean);
      const phones=split(p.sellerPhone).map(x=>`<a href="tel:${esc(x.replace(/[^0-9+]/g,''))}">${esc(x)}</a>`).join(', ');
      const emails=split(p.sellerEmail).map(x=>`<a href="mailto:${esc(x)}">${esc(x)}</a>`).join(', ');
      const profile=/^https?:\/\//i.test(p.sellerProfileUrl)?`<a href="${esc(p.sellerProfileUrl)}" target="_blank" rel="noopener">${esc(c.profile)}</a>`:'';
      const contactRows=[p.sellerName||profile?[c.seller,[esc(p.sellerName),profile].filter(Boolean).join(' · ')]:null,phones?[c.phone,phones]:null,emails?[c.email,emails]:null,[c.seenIn,esc(sourcesOf(p).join(', '))]].filter(Boolean);
      const contact=`<div class="divider2"></div><div class="detail-label">${esc(c.contact)}</div><dl class="contact">${contactRows.map(([k,v])=>`<dt>${esc(k)}</dt><dd>${v}</dd>`).join('')}</dl>${phones||emails?'':`<p class="muted-note">${esc(c.noContact)}</p>`}`;
      const facts=[p.beds!=null?`${p.beds} bd`:'',p.baths!=null?`${p.baths} ba`:'',p.sqft?`${p.sqft.toLocaleString('en-US')} ${c.sqft}`:''].filter(Boolean).join(' · ');
      const reasons=(p.scoreReasons||[]).map(r=>{const[k,v]=r.split(':');const f=c.reasons[k];return f?f(v===undefined?undefined:Number(v)):r});
      const scoreLine=p.score==null?'':`<p class="formula"><b>${esc(c.scoreWhy)} ${p.score}</b> (${esc(p.scoreIsAuto?c.scoreAuto:c.scoreManual)})${reasons.length?': '+esc(reasons.join(' · ')):''}</p>`;
      const market=p.marketValue==null?'':`<div class="numbers market"><div class="num"><span>${esc(c.marketValue)}</span><b>${money(p.marketValue)}</b></div><div class="num"><span>${esc(c.discount)}</span><b class="${p.discount>0?'good':''}">${p.discount==null?'—':p.discount+'%'}</b></div></div>`;
      const steps=[es?'Confirmar dirección, fotos y condición de la propiedad.':'Confirm address, photos and property condition.',es?'Revisar ARV, repairs y oferta máxima.':'Review ARV, repairs and maximum offer.',es?'Contactar, agendar visita o preparar oferta.':'Contact seller, schedule visit or prepare offer.'];
      detail.innerHTML=`<div class="detail-label">${es?'Análisis de propiedad':'Property analysis'} · ${esc(p.publicId)}</div><h2>${esc(p.title)}</h2><div class="location">${esc(location)}${facts?' · '+esc(facts):''}</div>${origin}${gallery}${contact}${post}<div class="divider2"></div>${market}<div class="numbers"><div class="num"><span>${es?'Precio pedido':'Asking'}</span><b>${p.asking==null?esc(c.notPosted):money(p.asking)}</b></div><div class="num"><span>${es?'ARV estimado':'ARV estimate'}</span><b>${esc(range(p.arv,p.arvLow,p.arvHigh))}</b></div><div class="num"><span>${es?'Rango de repairs':'Repair range'}</span><b>${esc(range(p.repairs,p.repairsLow,p.repairsHigh))}</b></div><div class="num"><span>${es?'Estado financiero':'Financial fit'}</span><b class="score">${p.fit?esc(c.fit[p.fit]):'—'}</b></div></div><div class="offerbox"><span>${es?'Oferta máxima para nosotros':'Maximum offer for us'}</span><b>${p.maxOffer==null?esc(c.pendingData):money(p.maxOffer)}</b></div><p class="formula"><b>${es?'Por qué:':'Why:'}</b> ${esc(why(p))}</p>${scoreLine}<button class="action" id="review" ${inReview?'disabled':''}>${esc(inReview?c.reviewDone:es?'Marcar para revisión humana':'Mark for human review')}</button><button class="mutedbutton" id="calcFromLead">${es?'Probar fórmula de oferta':'Test the offer formula'}</button><div class="divider2"></div><div class="detail-label">${es?'Qué sigue':'What happens next'}</div><div class="stepper">${steps.map((s,i)=>`<div class="step ${i===step?'current':''}"><b>${i+1}</b><span>${s}</span></div>`).join('')}</div><div class="divider2"></div><div class="detail-label">${esc(c.history)}</div><ul class="history" id="history"></ul>`;
      detail.querySelectorAll('[data-photo]').forEach(a=>a.addEventListener('click',e=>{e.preventDefault();const hero=document.getElementById('heroPhoto');hero.href=a.href;hero.classList.remove('broken');hero.querySelector('img').src=a.href;detail.querySelectorAll('[data-photo]').forEach(x=>x.classList.toggle('on',x===a))}));
      const copy=document.getElementById('copyLink');
      if(copy)copy.onclick=()=>navigator.clipboard.writeText(link).then(()=>toast(c.linkCopied),()=>toast(link));
      const toggle=document.getElementById('togglePost');
      if(toggle)toggle.onclick=()=>{const open=document.getElementById('postText').classList.toggle('collapsed');toggle.textContent=open?c.showMore:c.showLess};
      document.getElementById('review').onclick=()=>markReview(p.id);
      document.getElementById('calcFromLead').onclick=()=>openCalc(p);
      loadHistory(p.id);
    }
    async function loadHistory(id){
      const list=document.getElementById('history');
      try{const r=await fetch(`/api/opportunity/properties/${id}/events`,{cache:'no-store'});const body=await r.json().catch(()=>({}));if(!r.ok)throw body;if(selected!==id||!list.isConnected)return;
        const ev=t().events;list.innerHTML=body.length?body.map(e=>`<li><span>${esc((ev[e.kind]||(()=>e.kind))(e.detail||{}))}</span><small>${esc(when(e.createdAt))}</small></li>`).join(''):`<li class="muted-note">${esc(t().historyEmpty)}</li>`}
      catch(e){if(list.isConnected)list.innerHTML=`<li class="muted-note">${esc(apiError(e))}</li>`}
    }
    async function markReview(id){
      const btn=document.getElementById('review');btn.disabled=true;
      const r=await fetch(`/api/opportunity/properties/${id}/review`,{method:'POST'});const body=await r.json().catch(()=>({}));
      if(!r.ok){toast(apiError(body));btn.disabled=false;return}
      replace(body);toast(t().reviewToast);render();
    }
    document.querySelectorAll('[data-view]').forEach(b=>b.addEventListener('click',()=>{current=b.dataset.view;selected=null;document.querySelectorAll('[data-view]').forEach(x=>x.classList.toggle('active',x===b));render()}));
    const modal=document.getElementById('modal'), calcError=document.getElementById('calcError'), saveBtn=document.getElementById('saveNumbers');
    function setInput(id,v){document.getElementById(id).value=v==null?'':String(v)}
    function openCalc(p){
      calcTarget=p&&p.id?p:null;calcError.hidden=true;
      if(calcTarget){setInput('arv',p.arv??(p.arvLow!=null&&p.arvHigh!=null?Math.round((p.arvLow+p.arvHigh)/2):null));setInput('repairs',p.repairs??(p.repairsLow!=null&&p.repairsHigh!=null?Math.round((p.repairsLow+p.repairsHigh)/2):null));setInput('closing',p.closingHolding);setInput('contingency',p.contingency)}
      document.getElementById('calcProperty').textContent=calcTarget?`${calcTarget.publicId} · ${calcTarget.title}`:'';
      saveBtn.hidden=!calcTarget;modal.classList.add('open');updateCalc();
    }
    function closeCalc(){modal.classList.remove('open')}
    document.getElementById('openCalc').onclick=()=>openCalc(null);document.getElementById('closeCalc').onclick=closeCalc;modal.addEventListener('click',e=>{if(e.target===modal)closeCalc()});
    function n(id){return Number(document.getElementById(id).value.replace(/[^0-9.]/g,''))||0}
    function optional(id){return document.getElementById(id).value.trim()===''?null:n(id)}
    function updateCalc(){const total=n('arv')*.65-n('repairs')-n('closing')-n('contingency');document.getElementById('result').textContent=money(Math.max(0,total))}
    ['arv','repairs','closing','contingency'].forEach(id=>document.getElementById(id).addEventListener('input',updateCalc));
    saveBtn.onclick=async()=>{
      if(!calcTarget)return;saveBtn.disabled=true;calcError.hidden=true;
      const r=await fetch(`/api/opportunity/properties/${calcTarget.id}/numbers`,{method:'PATCH',headers:{'Content-Type':'application/json'},body:JSON.stringify({arv:optional('arv'),repairs:optional('repairs'),closingHolding:optional('closing'),contingency:optional('contingency')})});
      const body=await r.json().catch(()=>({}));saveBtn.disabled=false;
      if(!r.ok){calcError.textContent=apiError(body);calcError.hidden=false;return}
      replace(body);closeCalc();toast(t().savedToast);render();
    };
    document.getElementById('filterSource').onchange=e=>{fSource=e.target.value;selected=null;render()};
    document.getElementById('sortBy').onchange=e=>{sortBy=e.target.value;render()};

    const $=id=>document.getElementById(id);
    const scanModal=$('scanModal'), scanState=$('scanState'), scanCity=$('scanCity'), scanSlug=$('scanSlug'), scanSite=$('scanSite'), scanGroups=$('scanGroups'), scanLimit=$('scanLimit'), scanError=$('scanError'), runScan=$('runScan');
    let scanCfg=null, states=[], cities=[], slugEdited=false, siteEdited=false, scanTimer=null, lastTotals='';
    const slugOf=s=>s.toLowerCase().replace(/[^a-z0-9]/g,'');
    const showScanError=m=>{scanError.textContent=m;scanError.hidden=!m};
    async function getJson(url,opts){const r=await fetch(url,{cache:'no-store',...opts});const body=await r.json().catch(()=>({}));if(!r.ok)throw body;return body}
    const cityCache=new Map();
    async function citiesOf(state){if(!cityCache.has(state))cityCache.set(state,await getJson(`/api/opportunity/geo/cities?state=${encodeURIComponent(state)}`));return cityCache.get(state)}
    async function loadStates(){
      if(!states.length)states=await getJson('/api/opportunity/geo/states');
      scanState.innerHTML=states.map(s=>`<option value="${esc(s.code)}">${esc(s.name)} (${esc(s.code)})</option>`).join('');
    }
    async function loadCities(city){
      cities=await citiesOf(scanState.value);
      scanCity.innerHTML=`<option value="">${esc(t().scanChooseCity(cities.length))}</option>`+cities.map(c=>`<option value="${esc(c.name)}">${esc(c.name)}</option>`).join('');
      scanCity.value=city&&cities.some(c=>c.name===city)?city:'';cityChanged();
    }
    const filterState=$('filterState'), filterCity=$('filterCity');
    async function renderGeoFilters(){
      const c=t();
      fillSelect(filterState,[['',c.allStates],...states.map(s=>[s.code,`${s.name} (${s.code})`])],fState);
      if(!fState){fillSelect(filterCity,[['',c.pickStateFirst]],'');filterCity.disabled=true;return}
      const list=await citiesOf(fState);
      fillSelect(filterCity,[['',c.allCitiesN(list.length)],...list.map(x=>[x.name,x.name])],fCity);filterCity.disabled=false;
    }
    async function initGeo(){
      try{if(!states.length)states=await getJson('/api/opportunity/geo/states');await renderGeoFilters()}
      catch(e){fillSelect(filterState,[['',apiError(e)]],'')}
    }
    filterState.onchange=async()=>{fState=filterState.value;fCity='';selected=null;render();await renderGeoFilters()};
    filterCity.onchange=()=>{fCity=filterCity.value;selected=null;render()};
    function cityChanged(){const s=slugOf(scanCity.value);if(!slugEdited)scanSlug.value=s;if(!siteEdited)scanSite.value=s;updateEstimate()}
    const chosen=()=>[...document.querySelectorAll('#scanSources input:checked')].map(i=>i.value);
    function renderSources(){
      const box=$('scanSources'), c=t(), before=box.children.length?new Set(chosen()):null;
      box.innerHTML=scanCfg.sources.map(s=>`<label class="source-opt"><input type="checkbox" value="${esc(s.key)}" ${(before?before.has(s.key):s.selected)?'checked':''}><span>${esc(s.label)}${s.needsGroups?`<small>${esc(c.needsGroups)}</small>`:''}</span><em>$${s.costPer1000}${esc(c.perK)}</em></label>`).join('');
      box.querySelectorAll('input').forEach(i=>i.onchange=updateEstimate);
    }
    function updateEstimate(){
      if(!scanCfg)return;
      const limit=Math.max(0,Number(scanLimit.value)||0), picked=scanCfg.sources.filter(s=>chosen().includes(s.key));
      const per=s=>s.key==='x'?Math.max(limit,50):limit;
      const usd=picked.reduce((a,s)=>a+per(s)*s.costPer1000/1000,0), max=picked.reduce((a,s)=>a+per(s),0);
      $('scanEstimate').textContent=picked.length&&limit?t().scanEstimate(usd.toFixed(2),max):'';
    }
    function renderScans(){
      const c=t(), running=scanCfg.searches.some(s=>s.status==='Running');
      $('scanBadge').textContent=running?` · ${c.scanRunning}`:'';
      runScan.disabled=running||!scanCfg.configured;
      const noticeText=[scanCfg.configured?'':c.scanNoToken,scanCfg.dbError?c.scanNoDb:''].filter(Boolean).join(' ');
      const notice=$('scanNotice');notice.textContent=noticeText;notice.hidden=!noticeText;runScan.disabled=runScan.disabled||!!scanCfg.dbError;
      $('scanList').innerHTML=scanCfg.searches.length?scanCfg.searches.map(s=>`<li class="scan-item"><div><b>${esc(s.city)}, ${esc(s.state)}</b> · <span class="st ${esc(s.status)}">${esc(c.status[s.status]||s.status)}</span><small>${esc(when(s.startedAt))}</small></div><div class="muted-note">${esc(c.searchLine(s))}${s.error?' · '+esc(s.error):''}</div><ul>${Object.values(s.sources).map(p=>`<li><span class="st ${esc(p.status)}">${esc(c.status[p.status]||p.status)}</span> <b>${esc(p.label)}</b> · ${esc(c.sourceLine(p))}${p.error?`<div class="calc-error">${esc(p.error)}</div>`:''}</li>`).join('')}</ul></li>`).join(''):`<li class="muted-note">${esc(c.scanNone)}</li>`;
      return running;
    }
    async function loadScans(){
      try{scanCfg=await getJson('/api/opportunity/searches')}catch(e){showScanError(apiError(e));return}
      if(!$('scanSources').children.length){renderSources();if(!scanLimit.value)scanLimit.value=scanCfg.defaultLimit;scanLimit.max=scanCfg.maxLimit}
      const running=renderScans(), totals=scanCfg.searches.map(s=>`${s.id}:${s.imported}:${s.updated}:${s.status}`).join('|');
      if(lastTotals&&totals!==lastTotals)load();
      lastTotals=totals;updateEstimate();
      clearTimeout(scanTimer);if(running)scanTimer=setTimeout(loadScans,5000);
    }
    async function openScan(){
      scanModal.classList.add('open');showScanError('');
      try{
        await loadStates();
        scanState.value=fState||(states.some(s=>s.code==='NC')?'NC':states[0].code);
        await loadCities(fCity);
      }catch(e){showScanError(apiError(e))}
      await loadScans();
    }
    $('openScan').onclick=openScan;$('closeScan').onclick=()=>scanModal.classList.remove('open');scanModal.addEventListener('click',e=>{if(e.target===scanModal)scanModal.classList.remove('open')});
    scanState.onchange=()=>loadCities().catch(e=>showScanError(apiError(e)));
    scanCity.addEventListener('change',cityChanged);scanLimit.addEventListener('input',updateEstimate);
    scanSlug.addEventListener('input',()=>slugEdited=scanSlug.value!=='');scanSite.addEventListener('input',()=>siteEdited=scanSite.value!=='');
    runScan.onclick=async()=>{
      const c=t();showScanError('');
      if(!cities.some(x=>x.name.toLowerCase()===scanCity.value.trim().toLowerCase()))return showScanError(c.scanPickCity);
      if(!chosen().length)return showScanError(c.scanPickSource);
      runScan.disabled=true;
      try{
        await getJson('/api/opportunity/searches',{method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify({state:scanState.value,city:scanCity.value.trim(),sources:chosen(),limitPerSource:Number(scanLimit.value)||null,marketplaceSlug:scanSlug.value,craigslistSite:scanSite.value,facebookGroups:scanGroups.value.split('\n').map(x=>x.trim()).filter(Boolean)})});
        toast(c.scanStarted);await loadScans();
      }catch(e){showScanError(apiError(e));runScan.disabled=false}
    };
    document.getElementById('langToggle').onclick=()=>{lang=lang==='en'?'es':'en';applyLanguage();renderGeoFilters().catch(()=>{});if(scanCfg){renderSources();renderScans();updateEstimate()}};
    initGeo();
    getJson('/api/opportunity/searches').then(cfg=>{scanCfg=cfg;lastTotals=cfg.searches.map(s=>`${s.id}:${s.imported}:${s.updated}:${s.status}`).join('|');if(renderScans())loadScans()}).catch(()=>{});document.getElementById('roadmapNav').onclick=()=>document.getElementById('roadmap').scrollIntoView({behavior:'smooth',block:'start'});applyLanguage();load();
