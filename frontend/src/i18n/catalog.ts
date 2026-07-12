import type { Lang } from './index'

// Human-authored translations (not machine translation). Each key maps to one string per language.
// English is the source; a missing language falls back to English at lookup time.
export type Entry = Partial<Record<Lang, string>> & { en: string }

export const CATALOG: Record<string, Entry> = {
  // ---- app shell / sidebar / topbar ----
  'shell.portal': { en: 'PCI Global Portal', ko: 'PCI 글로벌 포털', ar: 'بوابة PCI العالمية', es: 'Portal PCI Global', fr: 'Portail PCI Global', zh: 'PCI 全球门户', ru: 'Портал PCI Global' },
  'shell.menu': { en: 'Menu', ko: '메뉴', ar: 'القائمة', es: 'Menú', fr: 'Menu', zh: '菜单', ru: 'Меню' },
  'shell.studentPortal': { en: 'Student Portal', ko: '학생 포털', ar: 'بوابة الطالب', es: 'Portal del estudiante', fr: 'Portail étudiant', zh: '学生门户', ru: 'Портал студента' },
  'shell.signOut': { en: 'Sign out', ko: '로그아웃', ar: 'تسجيل الخروج', es: 'Cerrar sesión', fr: 'Se déconnecter', zh: '退出登录', ru: 'Выйти' },
  'shell.member': { en: 'Member', ko: '회원', ar: 'عضو', es: 'Miembro', fr: 'Membre', zh: '会员', ru: 'Участник' },
  'shell.foundingMember': { en: 'Founding member', ko: '창립 회원', ar: 'عضو مؤسِّس', es: 'Miembro fundador', fr: 'Membre fondateur', zh: '创始会员', ru: 'Член-учредитель' },
  'shell.guest': { en: 'Guest account', ko: '게스트 계정', ar: 'حساب ضيف', es: 'Cuenta de invitado', fr: 'Compte invité', zh: '访客账户', ru: 'Гостевой аккаунт' },
  'shell.membershipActive': { en: 'Membership active', ko: '멤버십 활성', ar: 'العضوية نشطة', es: 'Membresía activa', fr: 'Adhésion active', zh: '会员有效', ru: 'Членство активно' },
  'shell.feesWaived': { en: 'Fees waived — founding cohort', ko: '수수료 면제 — 창립 기수', ar: 'الرسوم معفاة — الدفعة المؤسِّسة', es: 'Cuotas exentas — cohorte fundadora', fr: 'Frais exonérés — cohorte fondatrice', zh: '费用豁免 — 创始群体', ru: 'Взносы отменены — учредительная группа' },
  'shell.membershipNotActive': { en: 'Membership not active', ko: '멤버십 비활성', ar: 'العضوية غير نشطة', es: 'Membresía no activa', fr: 'Adhésion inactive', zh: '会员未激活', ru: 'Членство не активно' },

  // ---- primary navigation ----
  'nav.overview': { en: 'Overview', ko: '개요', ar: 'نظرة عامة', es: 'Resumen', fr: 'Aperçu', zh: '概览', ru: 'Обзор' },
  'nav.certifications': { en: 'Certifications', ko: '자격증', ar: 'الشهادات', es: 'Certificaciones', fr: 'Certifications', zh: '认证', ru: 'Сертификаты' },
  'nav.credentials': { en: 'Credentials', ko: '자격 증명', ar: 'الاعتمادات', es: 'Credenciales', fr: 'Attestations', zh: '证书', ru: 'Удостоверения' },
  'nav.cpd': { en: 'CPD', ko: '보수교육(CPD)', ar: 'التطوير المهني', es: 'DPC', fr: 'DPC', zh: '继续专业发展', ru: 'НПР' },
  'nav.billing': { en: 'Billing', ko: '결제', ar: 'الفواتير', es: 'Facturación', fr: 'Facturation', zh: '账单', ru: 'Оплата' },
  'nav.resources': { en: 'Resources', ko: '자료', ar: 'الموارد', es: 'Recursos', fr: 'Ressources', zh: '资源', ru: 'Ресурсы' },
  'nav.messages': { en: 'Messages', ko: '메시지', ar: 'الرسائل', es: 'Mensajes', fr: 'Messages', zh: '消息', ru: 'Сообщения' },
  'nav.support': { en: 'Support', ko: '지원', ar: 'الدعم', es: 'Soporte', fr: 'Assistance', zh: '支持', ru: 'Поддержка' },
  'nav.profile': { en: 'Profile', ko: '프로필', ar: 'الملف الشخصي', es: 'Perfil', fr: 'Profil', zh: '个人资料', ru: 'Профиль' },

  // ---- sign in ----
  'auth.signInSubtitle': { en: 'Sign in to manage your certification journey.', ko: '인증 여정을 관리하려면 로그인하세요.', ar: 'سجّل الدخول لإدارة رحلة اعتمادك.', es: 'Inicie sesión para gestionar su proceso de certificación.', fr: 'Connectez-vous pour gérer votre parcours de certification.', zh: '登录以管理您的认证历程。', ru: 'Войдите, чтобы управлять процессом сертификации.' },
  'auth.email': { en: 'Email address', ko: '이메일 주소', ar: 'البريد الإلكتروني', es: 'Correo electrónico', fr: 'Adresse e-mail', zh: '电子邮箱', ru: 'Электронная почта' },
  'auth.password': { en: 'Password', ko: '비밀번호', ar: 'كلمة المرور', es: 'Contraseña', fr: 'Mot de passe', zh: '密码', ru: 'Пароль' },
  'auth.signIn': { en: 'Sign in', ko: '로그인', ar: 'تسجيل الدخول', es: 'Iniciar sesión', fr: 'Se connecter', zh: '登录', ru: 'Войти' },
  'auth.signingIn': { en: 'Signing in…', ko: '로그인 중…', ar: 'جارٍ تسجيل الدخول…', es: 'Iniciando sesión…', fr: 'Connexion…', zh: '正在登录…', ru: 'Вход…' },
  'auth.forgot': { en: 'Forgot password?', ko: '비밀번호를 잊으셨나요?', ar: 'هل نسيت كلمة المرور؟', es: '¿Olvidó su contraseña?', fr: 'Mot de passe oublié ?', zh: '忘记密码？', ru: 'Забыли пароль?' },
  'auth.createAccount': { en: 'Create a free account', ko: '무료 계정 만들기', ar: 'إنشاء حساب مجاني', es: 'Crear una cuenta gratis', fr: 'Créer un compte gratuit', zh: '创建免费账户', ru: 'Создать бесплатный аккаунт' },
  'auth.unableSignIn': { en: 'Unable to sign in.', ko: '로그인할 수 없습니다.', ar: 'تعذّر تسجيل الدخول.', es: 'No se puede iniciar sesión.', fr: 'Impossible de se connecter.', zh: '无法登录。', ru: 'Не удалось войти.' },

  // ---- create account ----
  'reg.title': { en: 'Create your free account', ko: '무료 계정 만들기', ar: 'أنشئ حسابك المجاني', es: 'Cree su cuenta gratuita', fr: 'Créez votre compte gratuit', zh: '创建您的免费账户', ru: 'Создайте бесплатный аккаунт' },
  'reg.subtitle': { en: 'Join in under a minute — build your profile now, pay only when you choose to enrol.', ko: '1분 안에 가입하세요. 지금 프로필을 만들고, 등록할 때만 결제하세요.', ar: 'انضم في أقل من دقيقة — أنشئ ملفك الآن، وادفع فقط عند اختيارك التسجيل.', es: 'Regístrese en menos de un minuto: cree su perfil ahora y pague solo cuando decida inscribirse.', fr: 'Inscrivez-vous en moins d’une minute — créez votre profil maintenant, payez uniquement lorsque vous choisissez de vous inscrire.', zh: '一分钟内即可加入 — 立即创建您的资料，仅在选择报名时付费。', ru: 'Регистрация меньше чем за минуту — создайте профиль сейчас, платите только когда решите записаться.' },
  'reg.firstName': { en: 'First name', ko: '이름', ar: 'الاسم الأول', es: 'Nombre', fr: 'Prénom', zh: '名字', ru: 'Имя' },
  'reg.lastName': { en: 'Last name', ko: '성', ar: 'اسم العائلة', es: 'Apellido', fr: 'Nom', zh: '姓氏', ru: 'Фамилия' },
  'reg.password': { en: 'Password (8+ characters)', ko: '비밀번호(8자 이상)', ar: 'كلمة المرور (8 أحرف على الأقل)', es: 'Contraseña (8+ caracteres)', fr: 'Mot de passe (8+ caractères)', zh: '密码（8个字符以上）', ru: 'Пароль (от 8 символов)' },
  'reg.confirmPassword': { en: 'Confirm password', ko: '비밀번호 확인', ar: 'تأكيد كلمة المرور', es: 'Confirmar contraseña', fr: 'Confirmer le mot de passe', zh: '确认密码', ru: 'Подтвердите пароль' },
  'reg.passwordsNoMatch': { en: 'Passwords do not match.', ko: '비밀번호가 일치하지 않습니다.', ar: 'كلمتا المرور غير متطابقتين.', es: 'Las contraseñas no coinciden.', fr: 'Les mots de passe ne correspondent pas.', zh: '两次输入的密码不一致。', ru: 'Пароли не совпадают.' },
  'reg.country': { en: 'Country', ko: '국가', ar: 'الدولة', es: 'País', fr: 'Pays', zh: '国家/地区', ru: 'Страна' },
  'reg.selectCountry': { en: 'Select your country…', ko: '국가를 선택하세요…', ar: 'اختر دولتك…', es: 'Seleccione su país…', fr: 'Sélectionnez votre pays…', zh: '选择您的国家/地区…', ru: 'Выберите страну…' },
  'reg.contactPhone': { en: 'Contact phone', ko: '연락처 전화번호', ar: 'هاتف التواصل', es: 'Teléfono de contacto', fr: 'Téléphone de contact', zh: '联系电话', ru: 'Контактный телефон' },
  'reg.phonePlaceholder': { en: 'Phone number', ko: '전화번호', ar: 'رقم الهاتف', es: 'Número de teléfono', fr: 'Numéro de téléphone', zh: '电话号码', ru: 'Номер телефона' },
  'reg.code': { en: 'Code', ko: '코드', ar: 'الرمز', es: 'Código', fr: 'Indicatif', zh: '区号', ru: 'Код' },
  'reg.createBtn': { en: 'Create free account', ko: '무료 계정 만들기', ar: 'إنشاء حساب مجاني', es: 'Crear cuenta gratuita', fr: 'Créer un compte gratuit', zh: '创建免费账户', ru: 'Создать бесплатный аккаунт' },
  'reg.creating': { en: 'Creating account…', ko: '계정 생성 중…', ar: 'جارٍ إنشاء الحساب…', es: 'Creando cuenta…', fr: 'Création du compte…', zh: '正在创建账户…', ru: 'Создание аккаунта…' },
  'reg.haveAccount': { en: 'Already have an account?', ko: '이미 계정이 있으신가요?', ar: 'هل لديك حساب بالفعل؟', es: '¿Ya tiene una cuenta?', fr: 'Vous avez déjà un compte ?', zh: '已经有账户了？', ru: 'Уже есть аккаунт?' },
  'reg.selectCountryError': { en: 'Please select your country.', ko: '국가를 선택해 주세요.', ar: 'يُرجى اختيار دولتك.', es: 'Seleccione su país.', fr: 'Veuillez sélectionner votre pays.', zh: '请选择您的国家/地区。', ru: 'Пожалуйста, выберите страну.' },
  'reg.phoneError': { en: 'Please enter your contact phone number, including the country code.', ko: '국가 번호를 포함한 연락처 전화번호를 입력해 주세요.', ar: 'يُرجى إدخال رقم هاتفك مع رمز الدولة.', es: 'Introduzca su número de teléfono, incluido el código de país.', fr: 'Veuillez saisir votre numéro de téléphone, indicatif pays compris.', zh: '请输入含国家/地区代码的联系电话。', ru: 'Введите контактный телефон вместе с кодом страны.' },
  'reg.min8': { en: 'Password must be at least 8 characters.', ko: '비밀번호는 8자 이상이어야 합니다.', ar: 'يجب أن تتكون كلمة المرور من 8 أحرف على الأقل.', es: 'La contraseña debe tener al menos 8 caracteres.', fr: 'Le mot de passe doit comporter au moins 8 caractères.', zh: '密码至少需8个字符。', ru: 'Пароль должен содержать не менее 8 символов.' },

  // ---- auth brand pane ----
  'abp.headline': { en: 'The credential for the people who control projects.', ko: '프로젝트를 관리하는 사람들을 위한 자격증.', ar: 'الاعتماد المهني لمن يتحكّمون في المشاريع.', es: 'La credencial para quienes controlan los proyectos.', fr: 'La certification pour ceux qui pilotent les projets.', zh: '为掌控项目的人而设的资历认证。', ru: 'Квалификация для тех, кто управляет проектами.' },
  'abp.point1': { en: 'Free account — pay only when you choose to enrol', ko: '무료 계정 — 등록할 때만 결제', ar: 'حساب مجاني — ادفع فقط عند اختيارك التسجيل', es: 'Cuenta gratuita: pague solo cuando decida inscribirse', fr: 'Compte gratuit — payez uniquement lorsque vous vous inscrivez', zh: '免费账户 — 仅在选择报名时付费', ru: 'Бесплатный аккаунт — оплата только при записи' },
  'abp.point2': { en: 'Your full professional record: experience, qualifications, credentials', ko: '경력, 자격, 자격증을 아우르는 전체 전문 기록', ar: 'سجلّك المهني الكامل: الخبرة والمؤهّلات والاعتمادات', es: 'Su historial profesional completo: experiencia, titulaciones, credenciales', fr: 'Votre dossier professionnel complet : expérience, diplômes, certifications', zh: '您的完整职业记录：经验、资格与证书', ru: 'Ваш полный профессиональный профиль: опыт, квалификации, дипломы' },
  'abp.point3': { en: 'Exams, CPD, membership and support — one dashboard', ko: '시험, 보수교육, 멤버십, 지원 — 하나의 대시보드', ar: 'الامتحانات والتطوير المهني والعضوية والدعم — لوحة تحكم واحدة', es: 'Exámenes, DPC, membresía y soporte: un solo panel', fr: 'Examens, DPC, adhésion et assistance — un seul tableau de bord', zh: '考试、继续教育、会员与支持 — 一个面板', ru: 'Экзамены, НПР, членство и поддержка — единая панель' },

  // ---- generic ----
  'common.language': { en: 'Language', ko: '언어', ar: 'اللغة', es: 'Idioma', fr: 'Langue', zh: '语言', ru: 'Язык' },
}
