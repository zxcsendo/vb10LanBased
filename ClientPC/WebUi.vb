Imports System.Data.SqlClient
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Win32

Module WebUi
    Public Const SCHOOL_NAME As String = "Datamex College of Saint Adeline"
    Public Const SCHOOL_TAG As String = "Quality Education. A Brighter Future."

    Public Sub EnableModernBrowser(exeName As String)
        Try
            Dim keyPath As String = "Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"
            Using key As RegistryKey = Registry.CurrentUser.CreateSubKey(keyPath)
                key.SetValue(exeName, 11001, RegistryValueKind.DWord)
            End Using
        Catch
        End Try
    End Sub

    Public Function Esc(value As String) As String
        If value Is Nothing Then Return ""
        Return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("""", "&quot;")
    End Function

    Public Function SharedCss() As String
        Dim css As New StringBuilder()
        css.Append("*{box-sizing:border-box;margin:0;padding:0;}")
        css.Append("html,body{height:100%;}")
        css.Append("body{font-family:'Segoe UI','Trebuchet MS',sans-serif;background:#f4eef1;color:#1c1c1e;overflow:hidden;}")
        css.Append("button,input,select,textarea{font-family:inherit;}")
        css.Append("input::-ms-clear,input::-ms-reveal{display:none;width:0;height:0;}")
        css.Append("input::-webkit-search-cancel-button{-webkit-appearance:none;}")
        css.Append(".bg{position:fixed;inset:0;z-index:0;")
        css.Append("background:radial-gradient(ellipse at 20% 0%,rgba(128,0,32,0.16),transparent 45%),")
        css.Append("radial-gradient(ellipse at 90% 10%,rgba(128,0,32,0.10),transparent 40%),")
        css.Append("linear-gradient(180deg,#fff 0%,#f7f1f3 55%,#f0e6ea 100%);}")
        css.Append(".stage{position:relative;z-index:1;height:100%;}")
        css.Append(".floatnav-wrap{position:absolute;top:16px;left:0;right:0;text-align:center;z-index:40;}")
        css.Append(".floatnav{display:inline-block;background:rgba(70,0,18,0.82);border:1px solid rgba(255,255,255,0.18);")
        css.Append("border-radius:999px;padding:7px 10px;box-shadow:0 12px 30px rgba(70,0,18,0.28);}")
        css.Append(".navbtn{display:inline-block;border:none;cursor:pointer;background:transparent;color:rgba(255,255,255,0.72);")
        css.Append("padding:10px 18px;margin:0 2px;border-radius:999px;font-size:12px;font-weight:bold;letter-spacing:0.4px;}")
        css.Append(".navbtn:hover{color:#fff;background:rgba(255,255,255,0.10);box-shadow:0 0 14px rgba(255,170,185,0.35);}")
        css.Append(".navbtn.active{color:#fff;background:rgba(255,255,255,0.18);box-shadow:0 0 18px rgba(255,150,170,0.55);}")
        css.Append(".brand-top{position:absolute;top:22px;left:22px;z-index:41;}")
        css.Append(".brand-top .name{font-family:Georgia,'Times New Roman',serif;font-size:15px;color:#800020;}")
        css.Append(".content{padding:78px 22px 22px;height:100%;overflow:auto;}")
        css.Append(".content .stats,.content .panel{width:100%;}")
        css.Append(".welcome{font-family:Georgia,serif;font-size:26px;color:#800020;margin-bottom:6px;}")
        css.Append(".welcome span{font-family:'Segoe UI',sans-serif;font-size:13px;color:#6e6e76;margin-left:8px;}")
        css.Append(".stats{margin-bottom:14px;}")
        css.Append(".stat{display:inline-block;width:23%;min-width:120px;background:rgba(255,255,255,0.88);border:1px solid #eadfe3;")
        css.Append("border-radius:14px;padding:14px;margin-right:1.5%;box-shadow:0 10px 24px rgba(70,10,25,0.07);}")
        css.Append(".stat .k{font-size:10px;color:#7a7276;letter-spacing:0.8px;text-transform:uppercase;}")
        css.Append(".stat .v{font-family:Georgia,serif;font-size:26px;color:#800020;margin-top:4px;}")
        css.Append(".panel{background:rgba(255,255,255,0.92);border:1px solid #eadfe3;border-radius:16px;padding:16px;")
        css.Append("box-shadow:0 14px 32px rgba(70,10,25,0.08);width:100%;}")
        css.Append(".filters{margin-bottom:12px;}")
        css.Append(".filters label{display:inline-block;font-size:11px;color:#6e6e76;margin:0 6px 0 0;}")
        css.Append(".filters select,.filters input{height:34px;border:1px solid #e3d6db;border-radius:999px;")
        css.Append("padding:0 12px;font-size:12px;margin-right:8px;background:#fff;}")
        css.Append(".filters input{width:170px;}")
        css.Append(".filters select{width:118px;}")
        css.Append(".table-wrap{overflow:auto;max-height:320px;border:1px solid #f0e6ea;border-radius:12px;}")
        css.Append("table.data{width:100%;border-collapse:collapse;font-size:12px;}")
        css.Append("table.data th{background:#800020;color:#fff;text-align:left;padding:10px 12px;}")
        css.Append("table.data td{padding:10px 12px;border-bottom:1px solid #f3e8ec;}")
        css.Append("table.data tr:nth-child(even) td{background:#fcfafb;}")
        css.Append("table.data tr:hover td{background:#fff5f7;}")
        css.Append(".badge{display:inline-block;padding:3px 8px;border-radius:999px;font-size:11px;font-weight:bold;background:#e8f7ee;color:#167846;}")
        css.Append(".btn{border:none;cursor:pointer;border-radius:999px;height:34px;padding:0 14px;font-size:12px;font-weight:bold;}")
        css.Append(".btn-view{background:rgba(255,255,255,0.9);color:#800020;border:1px solid rgba(128,0,32,0.28);}")
        css.Append(".btn-view:hover{background:#800020;color:#fff;box-shadow:0 0 16px rgba(128,0,32,0.35);}")
        css.Append(".btn-primary{background:#800020;color:#fff;height:44px;padding:0 22px;font-size:13px;")
        css.Append("box-shadow:0 10px 22px rgba(128,0,32,0.28);}")
        css.Append(".btn-primary:hover{background:#5c0017;}")
        css.Append(".btn-ghost{background:rgba(255,255,255,0.85);color:#800020;border:1px solid rgba(128,0,32,0.22);")
        css.Append("height:44px;padding:0 18px;margin-left:8px;}")
        css.Append(".empty{padding:30px;text-align:center;color:#6e6e76;}")
        css.Append(".modal-bg{position:fixed;left:0;top:0;right:0;bottom:0;background:rgba(28,10,14,0.55);display:none;z-index:100;}")
        css.Append(".modal-bg.show{display:block;}")
        css.Append(".modal{width:520px;max-width:92%;background:#fff;border-radius:16px;margin:6% auto 0;")
        css.Append("box-shadow:0 24px 60px rgba(0,0,0,0.28);overflow:hidden;}")
        css.Append(".modal-h{background:linear-gradient(90deg,#800020,#5a0018);color:#fff;padding:16px 18px;}")
        css.Append(".modal-h h2{font-family:Georgia,serif;font-size:20px;font-weight:normal;}")
        css.Append(".modal-h p{opacity:0.8;font-size:12px;margin-top:4px;}")
        css.Append(".modal-b{padding:18px;}")
        css.Append(".kv{width:100%;border-collapse:collapse;}")
        css.Append(".kv th{text-align:left;width:140px;padding:8px 0;color:#6e6e76;font-size:12px;font-weight:normal;}")
        css.Append(".kv td{padding:8px 0;font-size:13px;color:#1c1c1e;border-bottom:1px solid #f3e8ec;}")
        css.Append(".modal-f{padding:12px 18px 18px;text-align:right;}")
        css.Append(".login-wrap{height:100%;display:table;width:100%;}")
        css.Append(".login-cell{display:table-cell;vertical-align:middle;text-align:center;}")
        css.Append(".login-card{display:inline-block;width:420px;text-align:left;background:rgba(255,255,255,0.94);")
        css.Append("border:1px solid #eadfe3;border-radius:18px;padding:28px;box-shadow:0 20px 50px rgba(70,10,25,0.14);}")
        css.Append(".login-card h1{font-family:Georgia,serif;font-size:24px;color:#800020;font-weight:normal;}")
        css.Append(".login-card .school{font-size:12px;color:#6e6e76;margin:6px 0 18px;}")
        css.Append(".login-card label{display:block;font-size:11px;color:#6e6e76;margin:10px 0 5px;}")
        css.Append(".login-card input{width:100%;height:42px;border:1px solid #e3d6db;border-radius:999px;padding:0 14px;font-size:13px;}")
        css.Append(".login-card input:focus{outline:none;border-color:#800020;box-shadow:0 0 0 3px rgba(128,0,32,0.12);}")
        css.Append(".pass-wrap{position:relative;}")
        css.Append(".pass-wrap input{padding-right:48px;}")
        css.Append(".showpass{position:absolute;right:8px;top:6px;border:none;background:transparent;color:#800020;")
        css.Append("cursor:pointer;height:30px;width:34px;padding:0;line-height:0;}")
        css.Append(".showpass svg{width:20px;height:20px;display:inline-block;vertical-align:middle;}")
        css.Append(".login-err{color:#800020;font-size:12px;min-height:18px;margin-top:10px;}")
        css.Append(".chart-panel{margin-top:14px;width:100%;}")
        css.Append(".chart-head{margin-bottom:10px;}")
        css.Append(".chart-title{font-size:13px;color:#2a2a2e;font-weight:bold;display:inline-block;}")
        css.Append(".chart-range{float:right;}")
        css.Append(".rbtn{border:1px solid rgba(128,0,32,0.25);background:rgba(255,255,255,0.9);color:#800020;")
        css.Append("border-radius:999px;height:28px;padding:0 12px;font-size:11px;font-weight:bold;cursor:pointer;margin-left:4px;}")
        css.Append(".rbtn:hover{box-shadow:0 0 10px rgba(128,0,32,0.2);}")
        css.Append(".rbtn.on{background:#800020;color:#fff;border-color:#800020;}")
        css.Append(".chart-box{width:100%;height:230px;background:linear-gradient(180deg,#fff,#fcf8f9);border:1px solid #eadfe3;")
        css.Append("border-radius:14px;padding:12px 10px 8px;clear:both;}")
        css.Append(".chart-svg{width:100%;height:190px;display:block;}")
        css.Append(".field-err{border-color:#800020;box-shadow:0 0 0 3px rgba(128,0,32,0.12);}")
        css.Append(".hint-err{min-height:16px;font-size:11px;color:#800020;margin-top:4px;line-height:1.3;}")
        css.Append(".page-student{height:100%;overflow:auto;}")
        css.Append(".wrap{max-width:860px;margin:0 auto;padding:22px 18px 30px;}")
        css.Append(".hero h1{font-family:Georgia,serif;font-size:30px;font-weight:normal;color:#800020;}")
        css.Append(".card{background:rgba(255,255,255,0.94);border:1px solid #eadfe3;border-radius:16px;padding:22px;")
        css.Append("margin-top:16px;box-shadow:0 16px 36px rgba(70,10,25,0.08);}")
        css.Append(".section{font-size:12px;font-weight:bold;color:#800020;text-transform:uppercase;letter-spacing:1px;margin:6px 0 12px;}")
        css.Append(".grid{width:100%;border-collapse:separate;border-spacing:12px 0;margin-left:-12px;}")
        css.Append(".grid td{vertical-align:top;width:50%;padding-bottom:12px;}")
        css.Append("label.f{display:block;font-size:11px;color:#6e6e76;margin-bottom:5px;}")
        css.Append("input.f,select.f,textarea.f{width:100%;height:40px;border:1px solid #e3d6db;border-radius:999px;padding:0 12px;font-size:13px;background:#fff;outline:none;}")
        css.Append("textarea.f{height:78px;padding-top:9px;resize:none;border-radius:14px;}")
        css.Append("input.f:focus,select.f:focus,textarea.f:focus{border-color:#800020;box-shadow:0 0 0 3px rgba(128,0,32,0.12);}")
        css.Append(".msg{display:inline-block;margin-left:12px;font-size:12px;color:#6e6e76;}")
        css.Append(".msg.ok{color:#167846;}.msg.err{color:#800020;}")
        css.Append(".view-dash,.view-records{display:none;}")
        css.Append(".view-dash.show,.view-records.show{display:block;}")
        Return css.ToString()
    End Function

    Public Function StudentPageHtml() As String
        Dim sb As New StringBuilder()
        sb.Append("<!DOCTYPE html><html><head><meta http-equiv='X-UA-Compatible' content='IE=edge'/>")
        sb.Append("<meta charset='utf-8'/><style>").Append(SharedCss()).Append("</style></head><body>")
        sb.Append("<div class='bg'></div><div class='stage page-student'>")
        sb.Append("<div class='brand-top'><div class='name'>").Append(Esc(SCHOOL_NAME)).Append("</div></div>")
        sb.Append("<div class='wrap' style='padding-top:70px;'><div class='hero'><h1>Student registration</h1></div>")
        sb.Append("<div class='card'><form onsubmit='return saveStudent();' novalidate='novalidate'>")

        sb.Append("<div class='section'>Student identity</div><table class='grid'><tr>")
        sb.Append("<td><label class='f'>Student Number *</label><input class='f' id='num' maxlength='30' placeholder='e.g. 2026-0001'/>")
        sb.Append("<div class='hint-err' id='err-num'></div></td>")
        sb.Append("<td><label class='f'>First Name *</label><input class='f' id='fn' maxlength='50' placeholder='Enter first name'/>")
        sb.Append("<div class='hint-err' id='err-fn'></div></td></tr><tr>")
        sb.Append("<td><label class='f'>Last Name *</label><input class='f' id='ln' maxlength='50' placeholder='Enter last name'/>")
        sb.Append("<div class='hint-err' id='err-ln'></div></td>")
        sb.Append("<td><label class='f'>Middle Name</label><input class='f' id='mn' maxlength='50' placeholder='Optional'/>")
        sb.Append("<div class='hint-err' id='err-mn'></div></td></tr></table>")

        sb.Append("<div class='section'>Academic details</div><table class='grid'><tr>")
        sb.Append("<td><label class='f'>Course *</label><select class='f' id='course'>")
        sb.Append("<option value=''>Select course</option><option>BSIT</option><option>BSCS</option><option>BSIS</option><option>BSBA</option>")
        sb.Append("<option>BSED</option><option>BEED</option><option>BSCrim</option><option>BSHM</option></select>")
        sb.Append("<div class='hint-err' id='err-course'></div></td>")
        sb.Append("<td><label class='f'>Year Level *</label><select class='f' id='year'>")
        sb.Append("<option value=''>Select year</option><option>1st Year</option><option>2nd Year</option><option>3rd Year</option><option>4th Year</option></select>")
        sb.Append("<div class='hint-err' id='err-year'></div></td></tr><tr>")
        sb.Append("<td><label class='f'>Section *</label><input class='f' id='sec' maxlength='20' placeholder='e.g. A'/>")
        sb.Append("<div class='hint-err' id='err-sec'></div></td>")
        sb.Append("<td><label class='f'>Gender *</label><select class='f' id='gender'><option value=''>Select gender</option><option>Male</option><option>Female</option>")
        sb.Append("<option>Prefer not to say</option></select>")
        sb.Append("<div class='hint-err' id='err-gender'></div></td></tr></table>")

        sb.Append("<div class='section'>Contact</div><table class='grid'><tr>")
        sb.Append("<td><label class='f'>Contact Number *</label><input class='f' id='contact' maxlength='11' placeholder='09XXXXXXXXX'/>")
        sb.Append("<div class='hint-err' id='err-contact'></div></td>")
        sb.Append("<td><label class='f'>Email *</label><input class='f' id='email' maxlength='100' placeholder='name@email.com'/>")
        sb.Append("<div class='hint-err' id='err-email'></div></td></tr><tr>")
        sb.Append("<td colspan='2'><label class='f'>Address *</label><textarea class='f' id='addr' maxlength='255' placeholder='House no., street, barangay, city'></textarea>")
        sb.Append("<div class='hint-err' id='err-addr'></div></td></tr></table>")

        sb.Append("<div style='margin-top:8px;'>")
        sb.Append("<button type='submit' class='btn btn-primary'>Submit registration</button>")
        sb.Append("<button type='button' class='btn btn-ghost' onclick='clearForm()'>Clear</button>")
        sb.Append("<span id='status' class='msg'></span></div></form></div></div></div>")

        sb.Append("<script>")
        sb.Append("function val(id){return document.getElementById(id).value.replace(/^\s+|\s+$/g,'');}")
        sb.Append("function setErr(id,msg){var el=document.getElementById(id);var er=document.getElementById('err-'+id);")
        sb.Append("el.className=el.className.replace(' field-err','');if(msg){el.className+=' field-err';er.innerText=msg;}else{er.innerText='';}}")
        sb.Append("function setStatus(m,ok){var s=document.getElementById('status');s.className='msg '+(ok?'ok':'err');s.innerText=m;}")
        sb.Append("function clearForm(){['num','fn','ln','mn','sec','contact','email','addr'].forEach(function(i){document.getElementById(i).value='';setErr(i,'');});")
        sb.Append("['course','year','gender'].forEach(function(i){document.getElementById(i).selectedIndex=0;setErr(i,'');});setStatus('',true);}")
        sb.Append("document.getElementById('contact').onkeypress=function(e){var k=e.keyCode||e.which;if(k<48||k>57){e.returnValue=false;return false;}};")
        sb.Append("document.getElementById('contact').onkeyup=function(){this.value=this.value.replace(/[^0-9]/g,'').substring(0,11);};")
        sb.Append("function validate(){var ok=true;")
        sb.Append("function req(id,label){if(!val(id)){setErr(id,label+' is required.');ok=false;}else{setErr(id,'');}}")
        sb.Append("req('num','Student number');req('fn','First name');req('ln','Last name');")
        sb.Append("req('course','Course');req('year','Year level');req('sec','Section');req('gender','Gender');")
        sb.Append("req('contact','Contact number');req('email','Email');req('addr','Address');")
        sb.Append("var nm=val('num');if(nm && nm.length<3){setErr('num','Student number must be at least 3 characters.');ok=false;}")
        sb.Append("var ct=val('contact');if(ct){if(!/^[0-9]+$/.test(ct)){setErr('contact','Digits only (0-9).');ok=false;}")
        sb.Append("else if(ct.length!==11){setErr('contact','Contact must be exactly 11 digits.');ok=false;}")
        sb.Append("else if(ct.charAt(0)!=='0'){setErr('contact','Contact must start with 0.');ok=false;}}")
        sb.Append("var em=val('email');if(em && (em.indexOf('@')<1 || em.indexOf('.')<3)){setErr('email','Enter a valid email address.');ok=false;}")
        sb.Append("if(!ok)setStatus('Please fix the highlighted fields.',false);else setStatus('',true);return ok;}")
        sb.Append("function saveStudent(){if(!validate())return false;try{var r=window.external.SaveStudent(val('num'),val('fn'),val('ln'),val('mn'),val('course'),val('year'),val('sec'),val('gender'),val('contact'),val('email'),val('addr'));")
        sb.Append("if(r==='OK'){clearForm();setStatus('Registration saved.',true);}else{setStatus(r,false);}}catch(e){setStatus(e.message,false);}return false;}")
        sb.Append("</script></body></html>")
        Return sb.ToString()
    End Function

    Public Function RegistrarLoginHtml() As String
        Dim sb As New StringBuilder()
        sb.Append("<!DOCTYPE html><html><head><meta http-equiv='X-UA-Compatible' content='IE=edge'/>")
        sb.Append("<meta charset='utf-8'/><style>").Append(SharedCss()).Append("</style></head><body>")
        sb.Append("<div class='bg'></div><div class='stage'><div class='login-wrap'><div class='login-cell'>")
        sb.Append("<div class='login-card'><h1>Registrar Login</h1>")
        sb.Append("<div class='school'>").Append(Esc(SCHOOL_NAME)).Append("</div>")
        sb.Append("<form onsubmit='return doLogin();'>")
        sb.Append("<label>Username</label><input id='user' autocomplete='off'/>")
        sb.Append("<label>Password</label><div class='pass-wrap'><input id='pass' type='password'/>")
        sb.Append("<button type='button' class='showpass' id='btnShow' onclick='togglePass()' title='Show password'>")
        sb.Append("<svg id='eyeOpen' viewBox='0 0 24 24' fill='none' stroke='#800020' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'>")
        sb.Append("<path d='M1 12s4-7 11-7 11 7 11 7-4 7-11 7S1 12 1 12z'/><circle cx='12' cy='12' r='3'/></svg>")
        sb.Append("<svg id='eyeOff' style='display:none;' viewBox='0 0 24 24' fill='none' stroke='#800020' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'>")
        sb.Append("<path d='M17.94 17.94A10.94 10.94 0 0 1 12 19c-7 0-11-7-11-7a21.8 21.8 0 0 1 5.06-5.94'/>")
        sb.Append("<path d='M9.9 4.24A10.94 10.94 0 0 1 12 4c7 0 11 7 11 7a21.9 21.9 0 0 1-2.16 3.19'/>")
        sb.Append("<path d='M14.12 14.12a3 3 0 1 1-4.24-4.24'/><line x1='1' y1='1' x2='23' y2='23'/></svg>")
        sb.Append("</button></div>")
        sb.Append("<div class='login-err' id='err'></div>")
        sb.Append("<div style='margin-top:16px;'><button type='submit' class='btn btn-primary' style='width:100%;'>Sign in</button></div>")
        sb.Append("</form></div></div></div></div>")
        sb.Append("<script>")
        sb.Append("function togglePass(){var p=document.getElementById('pass');var o=document.getElementById('eyeOpen');var f=document.getElementById('eyeOff');")
        sb.Append("if(p.type==='password'){p.type='text';o.style.display='none';f.style.display='inline-block';}")
        sb.Append("else{p.type='password';o.style.display='inline-block';f.style.display='none';}}")

        sb.Append("function doLogin(){var u=document.getElementById('user').value.replace(/^\s+|\s+$/g,'');var p=document.getElementById('pass').value;")
        sb.Append("if(!u||!p){document.getElementById('err').innerText='Username and password are required.';return false;}")
        sb.Append("try{var r=window.external.Login(u,p);if(r!=='OK'){document.getElementById('err').innerText=r;} }catch(e){document.getElementById('err').innerText=e.message;}return false;}")
        sb.Append("</script></body></html>")
        Return sb.ToString()
    End Function

    Public Function RegistrarShellHtml() As String
        Dim sb As New StringBuilder()
        sb.Append("<!DOCTYPE html><html><head><meta http-equiv='X-UA-Compatible' content='IE=edge'/>")
        sb.Append("<meta charset='utf-8'/><style>").Append(SharedCss()).Append("</style></head><body>")
        sb.Append("<div class='bg'></div><div class='stage'>")
        sb.Append("<div class='brand-top'><div class='name'>").Append(Esc(SCHOOL_NAME)).Append("</div></div>")
        sb.Append("<div class='floatnav-wrap'><div class='floatnav'>")
        sb.Append("<button class='navbtn active' id='navDash' onclick=""goNav('dash')"">Home</button>")
        sb.Append("<button class='navbtn' id='navRecords' onclick=""goNav('records')"">Records</button>")
        sb.Append("<button class='navbtn' id='navLogout' onclick='doLogout()'>Logout</button>")
        sb.Append("</div></div>")

        sb.Append("<div class='content'>")
        sb.Append("<div id='viewDash' class='view-dash show'>")
        sb.Append("<div class='welcome'>Dashboard <span>Welcome back, Registrar</span></div>")
        sb.Append("<div class='stats'>")
        sb.Append("<div class='stat'><div class='k'>Total Students</div><div class='v' id='statTotal'>0</div></div>")
        sb.Append("<div class='stat'><div class='k'>Visible Now</div><div class='v' id='statVisible'>0</div></div>")
        sb.Append("<div class='stat'><div class='k'>Courses</div><div class='v' id='statCourses'>0</div></div>")
        sb.Append("<div class='stat'><div class='k'>Status</div><div class='v' style='font-size:16px;padding-top:8px;'>Ready</div></div>")
        sb.Append("</div>")
        sb.Append("<div class='panel chart-panel'><div class='chart-head'>")
        sb.Append("<div class='chart-title' id='chartTitle'>Registrations</div>")
        sb.Append("<div class='chart-range'>")
        sb.Append("<button type='button' class='rbtn on' id='rngWeeks' onclick=""setRange('weeks')"">Weeks</button>")
        sb.Append("<button type='button' class='rbtn' id='rngMonths' onclick=""setRange('months')"">Months</button>")
        sb.Append("<button type='button' class='rbtn' id='rngYears' onclick=""setRange('years')"">Years</button>")
        sb.Append("</div></div>")
        sb.Append("<div class='chart-box'><div id='chartArea'><div class='empty'>Loading chart...</div></div>")
        sb.Append("<div class='chart-legend' id='chartLegend'></div></div></div></div>")


        sb.Append("<div id='viewRecords' class='view-records'>")
        sb.Append("<div class='welcome'>Student Records</div>")
        sb.Append("<div class='panel' style='margin-top:10px;'>")
        sb.Append("<div class='filters'>")
        sb.Append("<label>Year</label><select id='fYear' onchange='applyFilters()'><option value=''>All</option>")
        sb.Append("<option>1st Year</option><option>2nd Year</option><option>3rd Year</option><option>4th Year</option></select>")
        sb.Append("<label>Course</label><select id='fCourse' onchange='applyFilters()'><option value=''>All</option>")
        sb.Append("<option>BSIT</option><option>BSCS</option><option>BSIS</option><option>BSBA</option>")
        sb.Append("<option>BSED</option><option>BEED</option><option>BSCrim</option><option>BSHM</option></select>")
        sb.Append("<label>Section</label><input id='fSection' placeholder='e.g. A' onkeyup='applyFilters()'/>")
        sb.Append("<label>Search</label><input id='fSearch' placeholder='Name / student no.' onkeyup='applyFilters()'/>")
        sb.Append("</div>")
        sb.Append("<div class='table-wrap'><div id='grid'><div class='empty'>Loading records...</div></div></div>")
        sb.Append("</div></div>")

        sb.Append("</div>")
        sb.Append("<div class='modal-bg' id='modalBg' onclick='if(event.target.id===""modalBg"")closeModal();'>")
        sb.Append("<div class='modal'><div class='modal-h'><h2 id='mTitle'>Student profile</h2><p id='mSub'>Full record</p></div>")
        sb.Append("<div class='modal-b' id='mBody'></div>")
        sb.Append("<div class='modal-f'><button class='btn btn-ghost' onclick='closeModal()'>Close</button></div></div></div>")
        sb.Append("</div>")

        sb.Append("<script>")
        sb.Append("function goNav( wh ){")
        sb.Append("document.getElementById('navDash').className='navbtn'+(wh==='dash'?' active':'');")
        sb.Append("document.getElementById('navRecords').className='navbtn'+(wh==='records'?' active':'');")
        sb.Append("document.getElementById('viewDash').className='view-dash'+(wh==='dash'?' show':'');")
        sb.Append("document.getElementById('viewRecords').className='view-records'+(wh==='records'?' show':'');")
        sb.Append("if(wh==='records'){try{applyFilters();}catch(e){}}")
        sb.Append("}")
        sb.Append("function applyFilters(){try{window.external.ApplyFilters(document.getElementById('fYear').value,document.getElementById('fCourse').value,document.getElementById('fSection').value,document.getElementById('fSearch').value);}catch(e){}}")
        sb.Append("function viewStudent(id){try{var html=window.external.GetStudentDetail(id);document.getElementById('mBody').innerHTML=html;document.getElementById('modalBg').className='modal-bg show';}catch(e){alert(e.message);}}")
        sb.Append("function closeModal(){document.getElementById('modalBg').className='modal-bg';}")
        sb.Append("function doLogout(){try{window.external.Logout();}catch(e){alert(e.message);}}")
        sb.Append("function setRange(r){")
        sb.Append("document.getElementById('rngWeeks').className='rbtn'+(r==='weeks'?' on':'');")
        sb.Append("document.getElementById('rngMonths').className='rbtn'+(r==='months'?' on':'');")
        sb.Append("document.getElementById('rngYears').className='rbtn'+(r==='years'?' on':'');")
        sb.Append("try{window.external.SetChartRange(r);}catch(e){alert(e.message);}}")
        sb.Append("</script></body></html>")
        Return sb.ToString()
    End Function
End Module

<ComVisible(True)>
Public Class StudentPageHost
    Public Function SaveStudent(num As String, fn As String, ln As String, mn As String,
                                course As String, year As String, sec As String, gender As String,
                                contact As String, email As String, addr As String) As String
        If String.IsNullOrWhiteSpace(num) OrElse String.IsNullOrWhiteSpace(fn) OrElse
           String.IsNullOrWhiteSpace(ln) OrElse String.IsNullOrWhiteSpace(sec) OrElse
           String.IsNullOrWhiteSpace(course) OrElse String.IsNullOrWhiteSpace(year) OrElse
           String.IsNullOrWhiteSpace(gender) OrElse String.IsNullOrWhiteSpace(contact) OrElse
           String.IsNullOrWhiteSpace(email) OrElse String.IsNullOrWhiteSpace(addr) Then
            Return "Please complete all required fields."
        End If
        If num.Trim().Length < 3 Then Return "Student number is too short."
        If email.IndexOf("@"c) < 1 OrElse email.IndexOf("."c) < 3 Then Return "Enter a valid email address."
        Dim digits As String = ""
        For Each ch As Char In contact
            If Char.IsDigit(ch) Then digits &= ch
        Next
        If digits.Length <> 11 OrElse Not digits.StartsWith("0") Then
            Return "Contact must be exactly 11 digits and start with 0."
        End If
        contact = digits

        Try
            Using con As SqlConnection = Connection.OpenConnection()
                Dim sql As String =
                    "INSERT INTO Students " &
                    "(StudentNumber, FirstName, LastName, MiddleName, Course, YearLevel, Section, Gender, ContactNumber, Email, Address) " &
                    "VALUES (@num, @fn, @ln, @mn, @course, @year, @sec, @gender, @contact, @email, @addr)"
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@num", num.Trim())
                    cmd.Parameters.AddWithValue("@fn", fn.Trim())
                    cmd.Parameters.AddWithValue("@ln", ln.Trim())
                    cmd.Parameters.AddWithValue("@mn", If(String.IsNullOrWhiteSpace(mn), CType(DBNull.Value, Object), mn.Trim()))
                    cmd.Parameters.AddWithValue("@course", course)
                    cmd.Parameters.AddWithValue("@year", year)
                    cmd.Parameters.AddWithValue("@sec", sec.Trim())
                    cmd.Parameters.AddWithValue("@gender", gender)
                    cmd.Parameters.AddWithValue("@contact", If(String.IsNullOrWhiteSpace(contact), CType(DBNull.Value, Object), contact.Trim()))
                    cmd.Parameters.AddWithValue("@email", If(String.IsNullOrWhiteSpace(email), CType(DBNull.Value, Object), email.Trim()))
                    cmd.Parameters.AddWithValue("@addr", If(String.IsNullOrWhiteSpace(addr), CType(DBNull.Value, Object), addr.Trim()))
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            LiveSync.NotifyStudentsChanged()
            Return "OK"
        Catch ex As Exception
            Return "Database error: " & ex.Message
        End Try
    End Function
End Class

<ComVisible(True)>
Public Class RegistrarPageHost
    Private ReadOnly _form As frmRegistrar

    Public Sub New(form As frmRegistrar)
        _form = form
    End Sub

    Public Function Login(username As String, password As String) As String
        Return _form.TryLogin(username, password)
    End Function

    Public Sub Logout()
        _form.DoLogout()
    End Sub

    Public Sub ApplyFilters(year As String, course As String, section As String, search As String)
        _form.SetFilters(year, course, section, search)
        _form.RefreshData(False)
    End Sub

    Public Sub SetChartRange(rangeName As String)
        _form.SetChartRange(rangeName)
    End Sub

    Public Function GetStudentDetail(id As String) As String
        Return _form.BuildDetailHtml(id)
    End Function
End Class
