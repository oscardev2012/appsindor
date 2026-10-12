namespace CarolinasCoverage.Site;

internal static class GuideSeed
{
    private static readonly string[] ClaimEs =
    [
        "Protege a las personas y llama al 911 si existe una emergencia.",
        "Documenta fecha, lugar, fotos, datos y cualquier reporte disponible.",
        "Reporta el evento pronto por el canal de la aseguradora y conserva el número de reclamo.",
        "Un ajustador o la aseguradora puede revisar cobertura, daños y documentos antes de decidir."
    ];

    private static readonly string[] ClaimEn =
    [
        "Protect people and call 911 if there is an emergency.",
        "Document date, location, photos, details, and any available report.",
        "Report the event promptly through the insurer’s channel and keep the claim number.",
        "An adjuster or insurer may review coverage, damage, and documents before deciding."
    ];

    public static readonly Dictionary<string, GuideProduct> All = new()
    {
        ["auto"] = new("🚗",
            ["Progressive", "Nationwide", "Safeco / Liberty Mutual", "Dairyland", "National General", "Travelers", "Assurant America"],
            new("SEGURO DE AUTO", "Protección para los caminos de todos los días.",
                "El seguro de auto puede ayudar a atender responsabilidades frente a otras personas y, según la póliza elegida, daños a tu propio vehículo.",
                "Un viaje al trabajo, una visita a la familia o una parada rápida pueden cambiar en segundos. La conversación empieza con quién conduce, qué vehículo se protege y cómo se usa.",
                [
                    "Responsabilidad por lesiones o daños a terceros.",
                    "Daños a propiedad de terceros.",
                    "Conductor sin seguro o con seguro insuficiente, cuando esté disponible.",
                    "Colisión y cobertura integral/comprehensive para daños al vehículo, según se seleccione.",
                    "Opciones que pueden incluir asistencia en carretera, auto de renta, pagos médicos o protección de brecha, según disponibilidad."
                ], ClaimEs,
                [
                    "Licencia de conducir, vehículos y VIN cuando esté disponible.",
                    "Conductores del hogar, uso del vehículo e historial de seguro.",
                    "Cobertura actual, ubicación y cualquier requisito de préstamo o arrendamiento."
                ],
                "Los límites, deducibles, exclusiones, disponibilidad y precio dependen de la aseguradora, estado, elegibilidad y póliza final."),
            new("AUTO INSURANCE", "Protection for everyday roads.",
                "Auto insurance may help address responsibility to others and, depending on the policy selected, damage to your own vehicle.",
                "A commute, family visit, or quick errand can change in seconds. The conversation starts with who drives, which vehicle is protected, and how it is used.",
                [
                    "Liability for injuries or damage to others.",
                    "Damage to another person’s property.",
                    "Uninsured or underinsured motorist coverage, when available.",
                    "Collision and comprehensive coverage for vehicle damage, when selected.",
                    "Options that may include roadside help, rental reimbursement, medical payments, or gap protection, when available."
                ], ClaimEn,
                [
                    "Driver license, vehicles, and VIN when available.",
                    "Household drivers, vehicle use, and insurance history.",
                    "Current coverage, location, and any lender or lease requirement."
                ],
                "Limits, deductibles, exclusions, availability, and price depend on the insurer, state, eligibility, and final policy.")),

        ["home"] = new("🏠",
            ["Nationwide", "Safeco / Liberty Mutual", "Travelers", "JSA"],
            new("SEGURO DE HOGAR", "Una guía para proteger el lugar que llamas hogar.",
                "El seguro de hogar comúnmente reúne protecciones para la vivienda, pertenencias y responsabilidad personal, con condiciones y límites que deben revisarse en cada póliza.",
                "Una casa guarda años de trabajo, recuerdos y planes. Conocer el tipo de construcción, ubicación, techo, ocupación y riesgos ayuda a un agente a revisar opciones con más claridad.",
                [
                    "Vivienda: reparación o reconstrucción por ciertos daños cubiertos.",
                    "Otras estructuras, como una cerca o garaje separado, cuando la póliza lo contempla.",
                    "Propiedad personal y gastos adicionales de vivienda, bajo condiciones de la póliza.",
                    "Responsabilidad personal y pagos médicos a terceros, según corresponda.",
                    "Endosos que pueden modificar protección, deducibles o exclusiones."
                ], ClaimEs,
                [
                    "Dirección, año de construcción y características de la propiedad.",
                    "Aseguradora actual y reclamos de los últimos tres años.",
                    "Información de techo, ocupación, mejoras y riesgos relevantes."
                ],
                "Inundación, terremoto, desgaste, mantenimiento y otros eventos pueden tratarse de forma distinta o requerir opciones separadas. Lee siempre la póliza final."),
            new("HOME INSURANCE", "A guide to protecting the place you call home.",
                "Home insurance commonly combines protection for the dwelling, belongings, and personal liability, subject to each policy’s terms and limits.",
                "A home holds years of work, memories, and plans. Understanding construction, location, roof, occupancy, and risks helps an agent review options more clearly.",
                [
                    "Dwelling repair or rebuilding for certain covered damage.",
                    "Other structures, such as a fence or detached garage, when included.",
                    "Personal property and additional living expense, under policy conditions.",
                    "Personal liability and medical payments to others, as applicable.",
                    "Endorsements that may modify protection, deductibles, or exclusions."
                ], ClaimEn,
                [
                    "Address, year built, and property characteristics.",
                    "Current insurer and claims during the last three years.",
                    "Roof, occupancy, improvements, and relevant risk information."
                ],
                "Flood, earthquake, wear, maintenance, and other events may be treated differently or require separate options. Always read the final policy.")),

        ["renters"] = new("🔑",
            ["Progressive", "Safeco / Liberty Mutual", "Assurant America", "JSA"],
            new("SEGURO PARA INQUILINOS", "Tus cosas, tu responsabilidad y tu próximo paso.",
                "El seguro para inquilinos normalmente se enfoca en tus pertenencias y responsabilidad personal; la estructura del edificio suele ser responsabilidad del propietario.",
                "Rentar no significa que lo que tienes valga menos. Una conversación clara identifica tus pertenencias, la dirección y la responsabilidad que deseas considerar.",
                [
                    "Pertenencias personales ante ciertos eventos cubiertos.",
                    "Responsabilidad personal cuando alguien alega lesión o daño, según la póliza.",
                    "Pagos médicos a terceros en situaciones elegibles.",
                    "Gastos adicionales de vivienda si una pérdida cubierta vuelve inhabitable el lugar, cuando aplique.",
                    "Endosos para artículos de valor u otras necesidades, según disponibilidad."
                ], ClaimEs,
                [
                    "Dirección de renta y fecha de mudanza.",
                    "Valor aproximado de pertenencias y artículos especiales.",
                    "Requisitos del arrendador, si los hay."
                ],
                "La protección depende de la causa del daño, límites, deducible, exclusiones y términos de la póliza emitida."),
            new("RENTERS INSURANCE", "Your belongings, your liability, and your next step.",
                "Renters insurance normally focuses on your belongings and personal liability; the building itself is generally the landlord’s responsibility.",
                "Renting does not make what you own less valuable. A clear conversation identifies your belongings, address, and the liability protection you want to consider.",
                [
                    "Personal belongings for certain covered events.",
                    "Personal liability when someone alleges injury or damage, subject to the policy.",
                    "Medical payments to others in eligible situations.",
                    "Additional living expense if a covered loss makes the home unlivable, when applicable.",
                    "Endorsements for valuable items or other needs, when available."
                ], ClaimEn,
                [
                    "Rental address and move-in date.",
                    "Approximate value of belongings and special items.",
                    "Landlord requirements, if any."
                ],
                "Protection depends on the cause of loss, limits, deductible, exclusions, and terms of the issued policy.")),

        ["pet"] = new("🐾",
            ["Safeco / Liberty Mutual"],
            new("SEGURO PARA MASCOTAS", "Entender el cuidado antes de que llegue lo inesperado.",
                "El seguro para mascotas puede ayudar con gastos veterinarios elegibles. Normalmente se compara por tipo de plan, deducible, porcentaje de reembolso, límite anual, periodos de espera y exclusiones.",
                "Tu mascota es parte de la familia. La meta no es adivinar una factura veterinaria: es entender qué plan puede ajustarse a su edad, especie, historial y necesidades de cuidado.",
                [
                    "Planes que pueden enfocarse en accidentes y enfermedades.",
                    "Planes que pueden añadir bienestar preventivo o rutinario.",
                    "Opciones de deducible, porcentaje de reembolso y límite anual.",
                    "Tratamientos veterinarios elegibles según el plan y condiciones de la póliza.",
                    "Algunas pólizas pueden contemplar servicios específicos; confirma siempre qué aplica."
                ],
                [
                    "Busca atención veterinaria según la necesidad de tu mascota.",
                    "Conserva la factura detallada, expediente y comprobantes requeridos.",
                    "Presenta la solicitud a la aseguradora siguiendo su proceso y plazos.",
                    "La aseguradora revisa elegibilidad, deducible, reembolso, límite y exclusiones antes de decidir."
                ],
                [
                    "Perro o gato, raza, edad, sexo y si está esterilizado/a.",
                    "Fecha o confirmación de examen veterinario reciente, cuando aplique.",
                    "Dirección, datos de contacto e historial relevante permitido para cotizar."
                ],
                "Las condiciones preexistentes, periodos de espera, límites, exclusiones y reembolsos varían por plan, estado y póliza. Esta guía no confirma una cobertura específica."),
            new("PET INSURANCE", "Understanding care before the unexpected happens.",
                "Pet insurance may help with eligible veterinary expenses. Plans are commonly compared by plan type, deductible, reimbursement percentage, annual limit, waiting periods, and exclusions.",
                "Your pet is family. The goal is not to guess a veterinary bill; it is to understand which plan may fit the pet’s age, species, history, and care needs.",
                [
                    "Plans that may focus on accidents and illnesses.",
                    "Plans that may add preventive or routine wellness care.",
                    "Deductible, reimbursement-percentage, and annual-limit options.",
                    "Eligible veterinary treatment under the plan and policy conditions.",
                    "Some policies may address specific services; always confirm what applies."
                ],
                [
                    "Seek veterinary care according to your pet’s needs.",
                    "Keep the itemized invoice, records, and required receipts.",
                    "Submit the request to the insurer using its process and deadlines.",
                    "The insurer reviews eligibility, deductible, reimbursement, limit, and exclusions before deciding."
                ],
                [
                    "Dog or cat, breed, age, sex, and spay/neuter status.",
                    "Date or confirmation of a recent veterinary exam, when applicable.",
                    "Address, contact details, and permitted relevant history for a quote."
                ],
                "Pre-existing conditions, waiting periods, limits, exclusions, and reimbursement vary by plan, state, and policy. This guide does not confirm specific coverage.")),

        ["business"] = new("🏢",
            ["Progressive", "Safeco / Liberty Mutual", "JSA", "Berkshire Hathaway"],
            new("SEGURO PARA NEGOCIOS", "Protección pensada alrededor de cómo trabaja tu negocio.",
                "El seguro comercial se construye alrededor de la actividad, personas, propiedad, vehículos, contratos y riesgos de cada negocio. No existe un solo paquete correcto para todos.",
                "Un negocio puede comenzar en una camioneta, una oficina o una llamada. La pregunta correcta no es solo cuánto cuesta, sino qué podría pasar si una operación se interrumpe o alguien presenta un reclamo.",
                [
                    "Responsabilidad civil general para ciertas alegaciones de lesión o daño a terceros.",
                    "Compensación laboral cuando corresponde a empleados y requisitos aplicables.",
                    "Auto comercial para vehículos utilizados por la empresa.",
                    "Responsabilidad profesional, cyber o exceso/umbrella, cuando estén disponibles y sean pertinentes.",
                    "Certificados de seguro y requisitos contractuales que un agente puede revisar."
                ],
                [
                    "Cuida a las personas y documenta lo sucedido de forma objetiva.",
                    "Conserva contratos, fotos, reportes, nómina o registros pertinentes.",
                    "Reporta el evento al asegurador o agente conforme al proceso de la póliza.",
                    "No admitas responsabilidad ni aceptes acuerdos sin orientación de la aseguradora."
                ],
                [
                    "Nombre legal, dirección, año de inicio y tipo de operación.",
                    "Dueños, empleados, nómina, ingresos estimados y experiencia.",
                    "Vehículos, propiedades, contratos y cobertura actual."
                ],
                "La clasificación, auditorías, requisitos estatales, deducibles, límites, exclusiones y elegibilidad cambian según la actividad y la aseguradora.",
                ["general-liability", "commercial-auto", "workers-comp", "professional-liability", "cyber-liability", "umbrella", "excess-liability"]),
            new("BUSINESS INSURANCE", "Protection built around how your business works.",
                "Commercial insurance is built around a business’s activities, people, property, vehicles, contracts, and risks. There is no single right package for every business.",
                "A business may begin in a truck, office, or phone call. The better question is not only what it costs, but what could happen if operations stop or someone makes a claim.",
                [
                    "General liability for certain allegations of injury or third-party property damage.",
                    "Workers’ compensation when applicable to employees and relevant requirements.",
                    "Commercial auto for business-used vehicles.",
                    "Professional liability, cyber, or excess/umbrella liability when available and relevant.",
                    "Certificates of insurance and contractual requirements an agent may review."
                ],
                [
                    "Care for people and document what happened objectively.",
                    "Keep contracts, photos, reports, payroll, or relevant records.",
                    "Report the event to the insurer or agent under the policy process.",
                    "Do not admit liability or accept a settlement without insurer guidance."
                ],
                [
                    "Legal name, address, year started, and type of operations.",
                    "Owners, employees, payroll, estimated revenue, and experience.",
                    "Vehicles, property, contracts, and current coverage."
                ],
                "Classification, audits, state requirements, deductibles, limits, exclusions, and eligibility change by activity and insurer.",
                ["general-liability", "commercial-auto", "workers-comp", "professional-liability", "cyber-liability", "umbrella", "excess-liability"])),

        ["commercial-auto"] = new("🚚",
            ["Progressive", "National General", "JSA"],
            new("AUTO COMERCIAL", "Cuando el vehículo también es parte del trabajo.",
                "El auto comercial puede considerar vehículos, conductores, carga y uso empresarial. Lo que aplica depende de la operación, contrato y póliza.",
                "Una camioneta, un camión o una flota no solo se mueve: sostiene una operación. El uso, los conductores y la carga son parte de la conversación.",
                [
                    "Responsabilidad por lesiones y daños a terceros.",
                    "Daño físico al vehículo, cuando se elige.",
                    "Cobertura relacionada con carga u otras necesidades, cuando esté disponible.",
                    "Límites y requisitos que pueden depender de contratos, uso o regulación."
                ], ClaimEs,
                [
                    "Negocio, DOT cuando aplique, vehículo/VIN y valor.",
                    "Conductores, licencia, uso, carga y seguro actual."
                ],
                "No toda cobertura comercial incluye carga, remolques, conductores o usos específicos. Confírmalo en la póliza final."),
            new("COMMERCIAL AUTO", "When the vehicle is part of the work.",
                "Commercial auto may consider vehicles, drivers, cargo, and business use. What applies depends on operations, contracts, and the policy.",
                "A van, truck, or fleet does more than move: it supports an operation. Use, drivers, and cargo are part of the conversation.",
                [
                    "Liability for injury and third-party property damage.",
                    "Physical damage to the vehicle, when selected.",
                    "Cargo-related or other needs when available.",
                    "Limits and requirements that may depend on contracts, use, or regulation."
                ], ClaimEn,
                [
                    "Business, DOT when applicable, vehicle/VIN, and value.",
                    "Drivers, license, use, cargo, and current insurance."
                ],
                "Not every commercial policy includes cargo, trailers, drivers, or specific uses. Confirm this in the final policy.")),

        ["general-liability"] = new("🛡",
            ["Progressive", "Safeco / Liberty Mutual", "JSA", "Berkshire Hathaway"],
            new("RESPONSABILIDAD CIVIL GENERAL", "Cuando otra persona alega lesión o daño.",
                "La responsabilidad civil general puede ayudar a responder a ciertas alegaciones de lesión corporal, daño a propiedad de terceros o lesiones publicitarias, según la póliza y el hecho.",
                "Una visita a una obra, una entrega o una reunión con un cliente puede crear una pregunta de responsabilidad. La actividad del negocio y sus contratos dan contexto a cada opción.",
                [
                    "Lesiones corporales de terceros elegibles.",
                    "Daño a propiedad de terceros elegible.",
                    "Defensa frente a ciertas reclamaciones, bajo los términos de la póliza.",
                    "Responsabilidad por productos u operaciones terminadas, cuando aplique."
                ], ClaimEs,
                [
                    "Actividad y servicios del negocio.",
                    "Ubicaciones, contratos, ingresos y trabajo realizado.",
                    "Empleados, subcontratistas y cobertura actual."
                ],
                "No todas las pérdidas ni contratos se cubren. Límites, exclusiones y requisitos de seguro adicional se determinan por la póliza final."),
            new("GENERAL LIABILITY", "When another person alleges injury or damage.",
                "General liability may help respond to certain allegations of bodily injury, third-party property damage, or personal and advertising injury, depending on the policy and event.",
                "A jobsite visit, delivery, or client meeting can create a liability question. The business activity and contracts give context to each option.",
                [
                    "Eligible third-party bodily injury.",
                    "Eligible third-party property damage.",
                    "Defense against certain claims, under policy terms.",
                    "Products and completed-operations liability when applicable."
                ], ClaimEn,
                [
                    "Business activity and services.",
                    "Locations, contracts, revenue, and completed work.",
                    "Employees, subcontractors, and current coverage."
                ],
                "Not every loss or contract is covered. Limits, exclusions, and additional-insured requirements are defined by the final policy.")),

        ["workers-comp"] = new("🦺",
            ["Safeco / Liberty Mutual", "JSA", "Berkshire Hathaway"],
            new("COMPENSACIÓN LABORAL", "Una conversación importante para empleados y operación.",
                "La compensación laboral puede responder a lesiones o enfermedades laborales elegibles bajo la ley aplicable y la póliza. Los requisitos varían según estado, negocio y situación laboral.",
                "Cuando un equipo ayuda a crecer una empresa, su seguridad se vuelve parte de la operación. La clasificación del trabajo y la nómina son datos esenciales.",
                [
                    "Beneficios relacionados con lesiones o enfermedades de trabajo elegibles, conforme a ley y póliza.",
                    "Información de nómina y clasificación del trabajo para la cotización y auditoría.",
                    "Requisitos que pueden cambiar según estado, actividad y número de empleados."
                ],
                [
                    "Busca atención de emergencia cuando sea necesaria.",
                    "Informa el incidente conforme a la política interna y requisitos aplicables.",
                    "Conserva fecha, tarea, testigos y documentación; reporta al asegurador según el proceso."
                ],
                [
                    "Actividad empresarial, empleados, nómina y clasificación de trabajo.",
                    "Historial de reclamos, estado de operación y cobertura actual."
                ],
                "Esta guía no determina si una persona es empleado, contratista independiente ni la obligación legal de tener cobertura."),
            new("WORKERS’ COMPENSATION", "An important conversation for employees and operations.",
                "Workers’ compensation may respond to eligible work injuries or illnesses under applicable law and the policy. Requirements vary by state, business, and work arrangement.",
                "When a team helps grow a business, its safety becomes part of operations. Job classification and payroll are essential details.",
                [
                    "Benefits related to eligible work injuries or illnesses under law and the policy.",
                    "Payroll and work-classification information for quoting and audit.",
                    "Requirements that may change by state, activity, and number of employees."
                ],
                [
                    "Seek emergency care when needed.",
                    "Report the incident under internal policies and applicable requirements.",
                    "Keep the date, task, witnesses, and documentation; report to the insurer through its process."
                ],
                [
                    "Business activity, employees, payroll, and work classification.",
                    "Claims history, state of operation, and current coverage."
                ],
                "This guide does not determine whether someone is an employee or independent contractor, or the legal requirement to carry coverage.")),

        ["umbrella"] = new("☂",
            ["Nationwide", "Travelers"],
            new("RESPONSABILIDAD UMBRELLA", "Una capa adicional para responsabilidades grandes.",
                "Una póliza umbrella puede ofrecer una capa adicional de responsabilidad sobre ciertas pólizas subyacentes elegibles; no sustituye el seguro de auto o propiedad.",
                "Algunas situaciones superan los límites básicos. Un agente puede revisar responsabilidades, bienes y pólizas subyacentes antes de iniciar una conversación sobre umbrella.",
                [
                    "Puede ampliar protección de responsabilidad sobre pólizas subyacentes elegibles.",
                    "Puede considerar lesiones, daño a propiedad o ciertos reclamos personales, bajo sus términos.",
                    "Usualmente requiere límites mínimos en las pólizas subyacentes."
                ], ClaimEs,
                [
                    "Pólizas de auto, hogar o negocio existentes.",
                    "Límites actuales, riesgos y bienes que deseas conversar."
                ],
                "No cubre automáticamente daños a tu propio auto o vivienda. La póliza final define qué eventos, límites y exclusiones aplican."),
            new("UMBRELLA LIABILITY", "An extra layer for major liability.",
                "An umbrella policy may offer an extra layer of liability over certain eligible underlying policies; it does not replace auto or property insurance.",
                "Some situations exceed basic limits. An agent may review liability exposure, assets, and underlying policies before discussing umbrella coverage.",
                [
                    "May extend liability protection over eligible underlying policies.",
                    "May address injuries, property damage, or certain personal claims under its terms.",
                    "Usually requires minimum limits on the underlying policies."
                ], ClaimEn,
                [
                    "Existing auto, home, or business policies.",
                    "Current limits, risks, and assets you want to discuss."
                ],
                "It does not automatically cover damage to your own car or home. The final policy defines covered events, limits, and exclusions.")),

        ["excess-liability"] = new("↗",
            ["Berkshire Hathaway"],
            new("RESPONSABILIDAD EXCESO", "Límites adicionales para ciertos riesgos comerciales.",
                "La responsabilidad exceso puede proporcionar límites adicionales sobre coberturas subyacentes específicas cuando se cumplen sus condiciones. Su estructura puede ser diferente a una póliza umbrella.",
                "Contratos, proyectos grandes y activos de negocio pueden requerir una conversación más detallada sobre límites de responsabilidad y cómo encajan las pólizas existentes.",
                [
                    "Límites adicionales sobre coberturas subyacentes elegibles.",
                    "Revisión de pólizas primarias, límites requeridos y actividad del negocio.",
                    "Puede acompañar requisitos contractuales específicos, cuando la aseguradora lo aprueba."
                ], ClaimEs,
                [
                    "Pólizas subyacentes y límites actuales.",
                    "Contratos, proyectos, operaciones y requisitos de clientes."
                ],
                "El exceso no amplía automáticamente cada cobertura ni elimina exclusiones. La póliza final establece su alcance y desencadenantes."),
            new("EXCESS LIABILITY", "Additional limits for certain business risks.",
                "Excess liability may provide additional limits over specific underlying coverages when its conditions are met. Its structure may differ from an umbrella policy.",
                "Contracts, large projects, and business assets may call for a closer discussion of liability limits and how existing policies fit together.",
                [
                    "Additional limits over eligible underlying coverages.",
                    "Review of primary policies, required limits, and business activity.",
                    "May support specific contract requirements when approved by the insurer."
                ], ClaimEn,
                [
                    "Underlying policies and current limits.",
                    "Contracts, projects, operations, and client requirements."
                ],
                "Excess coverage does not automatically broaden every coverage or remove exclusions. The final policy establishes its scope and triggers.")),

        ["professional-liability"] = new("█",
            ["Berkshire Hathaway"],
            new("RESPONSABILIDAD PROFESIONAL", "Cuando el servicio profesional es parte de tu promesa.",
                "La responsabilidad profesional puede ayudar con ciertas alegaciones relacionadas con errores, omisiones o servicios profesionales, según la actividad y los términos de la póliza.",
                "El alcance de trabajo, contrato y servicio que presta el negocio importan tanto como el resultado. Por eso se revisa la actividad real, no solo el nombre de la empresa.",
                [
                    "Alegaciones relacionadas con ciertos errores u omisiones profesionales.",
                    "Gastos de defensa bajo términos y límites de la póliza.",
                    "Opciones que pueden considerar fechas retroactivas o reclamaciones hechas, cuando estén disponibles."
                ],
                [
                    "Conserva la comunicación, contrato, propuesta y cronología.",
                    "No admitas responsabilidad ni modifiques documentos sin orientación.",
                    "Reporta la alegación o circunstancia conforme a los requisitos de la póliza."
                ],
                [
                    "Servicios profesionales y licencias relevantes.",
                    "Contratos, ingresos, clientes, reclamaciones y cobertura previa."
                ],
                "No todos los servicios o daños son elegibles. La definición de servicio profesional, fecha de reclamación y exclusiones son esenciales en la póliza final."),
            new("PROFESSIONAL LIABILITY", "When professional service is part of your promise.",
                "Professional liability may help with certain allegations involving errors, omissions, or professional services, depending on the activity and policy terms.",
                "Scope of work, contract, and the service a business provides matter as much as the outcome. That is why the actual activity is reviewed, not only the company name.",
                [
                    "Allegations involving certain professional errors or omissions.",
                    "Defense expense under policy terms and limits.",
                    "Options that may consider retroactive dates or claims-made structures, when available."
                ],
                [
                    "Keep communications, contract, proposal, and timeline.",
                    "Do not admit liability or alter documents without guidance.",
                    "Report the allegation or circumstance under the policy’s requirements."
                ],
                [
                    "Professional services and relevant licenses.",
                    "Contracts, revenue, clients, claims, and prior coverage."
                ],
                "Not every service or damage is eligible. The professional-service definition, claim date, and exclusions are essential in the final policy.")),

        ["cyber-liability"] = new("⌘",
            ["Berkshire Hathaway"],
            new("RESPONSABILIDAD CYBER", "Prepararse para un incidente digital.",
                "La responsabilidad cyber puede abordar ciertos costos relacionados con incidentes de datos, redes o privacidad, sujeto a la póliza, controles y circunstancias del incidente.",
                "Un correo, una contraseña o un proveedor tecnológico pueden afectar una operación. Preparar contactos, copias de seguridad y un proceso de aviso forma parte de una conversación responsable.",
                [
                    "Respuesta a incidentes y ciertos gastos de investigación, cuando estén cubiertos.",
                    "Notificación, privacidad, interrupción de negocio o extorsión, según la póliza y disponibilidad.",
                    "Servicios especializados que pueden requerir aviso inmediato a la aseguradora."
                ],
                [
                    "Aísla el incidente de forma segura y conserva registros técnicos.",
                    "No elimines evidencia ni prometas pagos o notificaciones sin orientación.",
                    "Reporta el incidente de inmediato por el canal de la póliza y sigue la respuesta aprobada."
                ],
                [
                    "Datos que maneja el negocio, sistemas, proveedores y controles.",
                    "Ingresos, historial de incidentes y requisitos contractuales."
                ],
                "La cobertura cyber depende de definiciones, controles, exclusiones y notificación oportuna. Esta página no sustituye un plan de respuesta ni asesoría legal."),
            new("CYBER LIABILITY", "Preparing for a digital incident.",
                "Cyber liability may address certain costs related to data, network, or privacy incidents, subject to the policy, controls, and incident circumstances.",
                "An email, password, or technology provider can affect an operation. Preparing contacts, backups, and a reporting process is part of a responsible conversation.",
                [
                    "Incident response and certain investigation costs when covered.",
                    "Notification, privacy, business interruption, or extortion, depending on the policy and availability.",
                    "Specialized services that may require immediate insurer notice."
                ],
                [
                    "Safely isolate the incident and preserve technical records.",
                    "Do not destroy evidence or promise payment or notification without guidance.",
                    "Report the incident immediately through the policy channel and follow the approved response."
                ],
                [
                    "Business data, systems, vendors, and controls.",
                    "Revenue, incident history, and contract requirements."
                ],
                "Cyber coverage depends on definitions, controls, exclusions, and timely notice. This page does not replace an incident plan or legal advice."))
    };
}
