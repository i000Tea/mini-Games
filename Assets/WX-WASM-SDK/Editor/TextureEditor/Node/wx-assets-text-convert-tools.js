(()=>{"use strict";var t={28:(t,e,i)=>{var s=i(838),o=s.spawn,n=s.exec;function c(t,e,i){var s={};try{Object.keys(t).forEach((function(i){t[i].forEach((function(t){s[t]||(r(t,e),s[t]=1)})),s[i]||(r(i,e),s[i]=1)}))}catch(t){if(i)return i(t);throw t}if(i)return i()}function r(t,e){try{process.kill(parseInt(t,10),e)}catch(t){if("ESRCH"!==t.code)throw t}}function a(t,e,i,s,o){var n=s(t),c="";n.stdout.on("data",(function(t){t=t.toString("ascii"),c+=t})),n.on("close",(function(n){delete i[t],0==n?c.match(/\d+/g).forEach((function(n){n=parseInt(n,10),e[t].push(n),e[n]=[],i[n]=1,a(n,e,i,s,o)})):0==Object.keys(i).length&&o()}))}t.exports=function(t,e,i){if("function"==typeof e&&void 0===i&&(i=e,e=void 0),t=parseInt(t),Number.isNaN(t)){if(i)return i(new Error("pid must be a number"));throw new Error("pid must be a number")}var s={},r={};switch(s[t]=[],r[t]=1,process.platform){case"win32":n("taskkill /pid "+t+" /T /F",i);break;case"darwin":a(t,s,r,(function(t){return o("pgrep",["-P",t])}),(function(){c(s,e,i)}));break;default:a(t,s,r,(function(t){return o("ps",["-o","pid","--no-headers","--ppid",t])}),(function(){c(s,e,i)}))}}},838:t=>{t.exports=require("child_process")},936:t=>{t.exports=require("os")}},e={};function i(s){var o=e[s];if(void 0!==o)return o.exports;var n=e[s]={exports:{}};return t[s](n,n.exports,i),n.exports}i.n=t=>{var e=t&&t.__esModule?()=>t.default:()=>t;return i.d(e,{a:e}),e},i.d=(t,e)=>{for(var s in e)i.o(e,s)&&!i.o(t,s)&&Object.defineProperty(t,s,{enumerable:!0,get:e[s]})},i.o=(t,e)=>Object.prototype.hasOwnProperty.call(t,e),(()=>{const t=i(936),e={MACOS:"Darwin"===t.type(),M1:t.cpus().some((t=>t.model.toLowerCase().indexOf("apple")>-1)),DS:function(){switch(process.platform){case"darwin":return"/";case"win32":return"\\"}return"/"}()},s=i(838).spawn,o=i(28),n=[1,2,5],c={4096:200,2048:80,1024:70,512:60,256:40};class r{execPath="";options=[];#t=30;#e=()=>{};#i=()=>{};constructor(t=2048){for(let e in c)if(t>=e)return void(this.#t=c[e]);this.#t=30}run(){return new Promise(((t,e)=>{this.#e=t,this.#i=e,this.#s()}))}#s(){this.refreshTimeout()?(this.#o=!0,this.child=s(this.execPath,this.options),this.child.stdout.on("data",(function(t){})),this.child.on("close",(t=>{clearTimeout(this.#n),this.#e()}))):this.#i()}#c=-1;#n=-1;#o=!1;refreshTimeout(){return this.#c++,!(this.#c>=n.length||(this.#n=setTimeout((()=>{this.retryProcess()}),n[this.#c]*this.#t*1e3),0))}retryProcess(){this.#o&&(o(this.child.pid),this.#o=!1),this.#s()}}const a=require("fs");var h=i.n(a);const l={ProgressMessage(t,e,i){console.log(`[PROGRESS]${t},${e},${i}`)},ConsoleMessage(t){console.log(`[LOG]${t}`)},ResponseMsg(t){console.log(`[DONE]${t}`)},RecordFile(t){console.log(`[RECORDFILE]${t}`)}},u=e.MACOS?e.M1?"astcenc-neon":"astcenc-avx2":"astcenc-sse4.1.exe";class f extends r{#r="";#a="";#h="8x8";#l="-cs";constructor(t,i,s,o="8x8",n=2048,c="Gamma"){super(n),this.execPath=`${t}${e.DS}${u}`,this.#r=i,this.#a=s,this.#h=o,this.#l="Linear"==c?"-cl":"-cs"}async convert(){try{await h().promises.access(this.#a),h().rmSync(this.#a)}catch(t){}const t=this.#a+".astc";this.options=[this.#l,this.#r,t,this.#h,"-medium"],await this.run();try{h().renameSync(t,this.#a)}catch(t){l.ConsoleMessage(`【ASTC - convert】#104 原 [${this.#r}] 资源 ASTC 格式生成失败！`)}}}const g=e.MACOS?"PVRTexToolCLI":"PVRTexToolCLI.exe";class d extends r{#r="";#a="";#l="";constructor(t,i,s,o=2048,n="Gamma"){super(o),this.execPath=`${t}${e.DS}${g}`,this.#r=i,this.#a=s,this.#l="Linear"==n?"lRGB":"sRGB"}async convert(){try{await h().promises.access(this.#a),h().rmSync(this.#a)}catch(t){}const t=this.#a+".dds";this.options=["-i",this.#r,"-o",t,"-f",`BC3,UBN,${this.#l}`],await this.run();try{h().renameSync(t,this.#a)}catch(t){l.ConsoleMessage(`【DXT - convert】#106 原 [${this.#r}] 资源 DXT 格式生成失败！`)}}}const p=e.MACOS?"PVRTexToolCLI":"PVRTexToolCLI.exe";class S extends r{#r="";#a="";#l="";constructor(t,i,s,o=2048,n="Gamma"){super(o),this.execPath=`${t}${e.DS}${p}`,this.#r=i,this.#a=s,this.#l="Linear"==n?"lRGB":"sRGB"}async convert(){try{await h().promises.access(this.#a),h().rmSync(this.#a)}catch(t){}const t=this.#a+".pvr";this.options=["-i",this.#r,"-o",t,"-f",`ETC2_RGBA,UBN,${this.#l}`],await this.run();for(let e=0;e<10;e++){if(h().existsSync(t)){try{h().renameSync(t,this.#a);let e=h().readFileSync(this.#a);h().writeFileSync(this.#a,e.slice(52))}catch(t){l.ConsoleMessage(`【ETC2 - convert】#107 原 [${this.#r}] 资源 ETC2 格式生成失败！`)}return}await this.sleep(500)}l.ConsoleMessage(`【ETC2 - convert】#108 原 [${this.#r}] 资源 ETC2 格式生成失败！`)}sleep(t){return t<=0&&(t=1),new Promise((e=>{setTimeout((()=>{e()}),t)}))}}const $=e.MACOS?"pngquant":"pngquant.exe";class m extends r{#r="";#a="";constructor(t,i,s,o=2048,n="Gamma"){super(o),this.execPath=`${t}${e.DS}${$}`,this.#r=i,this.#a=s}async convert(){this.options=[this.#r,"-o",this.#r,"-f"],await this.run()}}const x=new class{#u;#f;#g;#d=!1;#p=[];constructor(){this.log("Wechat-assets-text-convert-tools Launch")}SetLogRoot(t){let i=new Date;const s=1e3+parseInt(8999*Math.random()),o=`WXLOGJS-${i.getHours()}-${i.getMinutes()}-${i.getSeconds()}-${s}.log`;let n=i.getMonth()+1;n<10&&(n="0"+n);let c=i.getDate();c<10&&(c="0"+c);const r=`${t}${e.DS}log${e.DS}${i.getFullYear()}${n}${c}`,a=`${r}${e.DS}${o}`;return this.#g=r,this.#u=a,this.#f=o,h().existsSync(this.#g)||h().mkdirSync(this.#g),h().existsSync(this.#u)||h().writeFileSync(this.#u,"",{encoding:"utf-8"}),this.#d=!0,this.#S(),this.#u}log(){if(!arguments||0==arguments.length)return;const t=arguments[0];let e=[];for(let t=1;t<arguments.length;t++)e.push(arguments[t]);let i={title:t,args:e};const s=JSON.stringify(i)+",";this.#$(s)}#m(){let t=new Date;t.getFullYear(),t.getMonth(),t.getDate(),t.getHours(),t.getMinutes(),t.getSeconds()}#$(t){this.#d?h().appendFileSync(this.#u,t,{encoding:"utf-8"}):this.#p.push(t)}#S(){for(;;){let t=this.#p.shift();if(!t)return;this.#$(t)}}};class P{#u;#g;#x={};constructor(t){this.#g=t,this.#u=`${t}${e.DS}JSDEALSFILERECORD.json`,this.#P(),this.#w()}#P(){h().existsSync(this.#g)||h().mkdirSync(this.#g),h().existsSync(this.#u)||h().writeFileSync(this.#u,"",{encoding:"utf-8"})}#w(){const t=h().readFileSync(this.#u,{encoding:"utf-8"});this.#x=""==t?{timestamp:0,files:[]}:JSON.parse(t)}save(){this.#x.timestamp=(new Date).getTime();const t=JSON.stringify(this.#x);h().existsSync(this.#u)&&h().rmSync(this.#u),h().writeFileSync(this.#u,t,{encoding:"utf-8"})}getInfo(t){for(let e of this.#x.files)if(e.originPngPath==t)return e;return null}setInfo(t){let e=!1,i=0;for(;i<this.#x.files.length;i++)if(this.#x.files[i].originPngPath==t.originPngPath){e=!0;break}e?this.#x.files[i]=t:this.#x.files.push(t)}defaultStruct(t,e,i){return{name:e.name,width:e.width,height:e.height,astc:!1,dxt:!1,etc:!1,pngmin:!1,lpng:!1,lpngmin:!1,originPngPath:t,colorSpace:i}}}const w=i(936).cpus().length,y=e.DS,C=new class{constructor(t){for(let e of t)this.#y(e)}#C={};getConfig(t){return this.#C[t]}setConfig(t,e){this.#C[t]=e}#y(t=""){let e=t.indexOf("=");if(e<=0||e>=t.length-1)return;let i=t.substring(0,e),s=t.substring(e+1,t.length);this.#C[i]=s}printOptions(){console.log("<-------------输入配置字典 Start-------------\x3e");for(let t in this.#C)console.log(`Key:${t} - Value:${this.#C[t]}`);console.log("<-------------输入配置字典 End-------------\x3e")}}(process.argv);new class{#D;#T=w;#p=[];#v=!1;#l="Gamma";#R=0;#O="";#M=!1;#b=0;#k;#F=0;constructor(t){this.#D=t}async launch(){this.#F=0,this.#I(),await this.#w(),this.#L=!1;for(let t=0;t<this.#T;t++)this.#N()}#I(){const t=this.#D.getConfig("-dataDir");if(!t)throw l.ConsoleMessage("【WXTools - convert】#201 缺少必要配置参数 -dataDir"),{code:201,errmsg:"缺少必要配置参数 -dataDir"};const e=x.SetLogRoot(t);l.RecordFile(e),this.#k=new P(t);const i=this.#D.getConfig("-config");if(!i)throw l.ConsoleMessage("【WXTools - convert】#201 缺少必要配置参数 -config"),{code:201,errmsg:"缺少必要配置参数 -config"};if(!h().existsSync(i))throw l.ConsoleMessage(`【WXTools - convert】#202 参数配置 -config [${i}] 指向无效`),{code:202,errmsg:`参数配置 -config [${i}] 指向无效`};if(!this.#D.getConfig("-outDir"))throw l.ConsoleMessage("【WXTools - convert】#203 缺少必要配置参数 -outDir"),{code:203,errmsg:"缺少必要配置参数 -outDir"};const s=this.#D.getConfig("-force");s&&(this.#M="true"==s);const o=["Gamma","Linear"],n=this.#D.getConfig("-colorSpace");n||(n="Gamma");let c=!1;for(let t of o)if(t==n){c=!0;break}if(!c)throw l.ConsoleMessage(`【WXTools - convert】#206 参数配置 -colorSpace [${n}] 赋值不合法`),{code:206,errmsg:`参数配置 -colorSpace [${n}] 赋值不合法`};this.#l=n;const r=this.#D.getConfig("-execRoot");if(!r)throw l.ConsoleMessage("【WXTools - convert】#204 缺少必要配置参数 -execRoot"),{code:204,errmsg:"缺少必要配置参数 -execRoot"};const a=["astcenc-avx2","astcenc-avx2.exe","astcenc-neon","astcenc-sse4.1.exe","pngquant","pngquant.exe","PVRTexToolCLI","PVRTexToolCLI.exe"];for(let t of a)if(!h().existsSync(`${r}${y}${t}`))throw l.ConsoleMessage(`【WXTools - convert】#205 参数配置 -execRoot [${r}] 缺少必要文件 [${t}]`),{code:204,errmsg:`参数配置 -execRoot [${r}] 缺少必要文件 [${t}]`};this.#O=r;const u=this.#D.getConfig("-type");this.#v=!(!u||"ASTC"!=u)}async#w(){const t=h().readFileSync(this.#D.getConfig("-config"),{encoding:"utf-8"}),e=JSON.parse(t),i=this.#D.getConfig("-outDir");for(let t of e.files)for(let e of t.textures){let t=decodeURIComponent(e.name),s=!1;for(let i of this.#p)if(i.name==t&&i.width==e.width){s=!0;break}if(s)continue;const o=`${i}${y}png${y}${e.width}${y}${t}.png`;if(!h().existsSync(o))continue;let n={name:t,width:e.width,height:e.height,astc:null==e.astc?"8x8":e.astc,limittype:e.limittype,path:`${i}${y}png${y}${e.width}${y}${t}.png`};h().existsSync(n.path)&&this.#p.push(n)}this.#R=this.#p.length;const s=`${i}${y}`;if(h().existsSync(`${s}astc`)||(this.#E=!0),h().existsSync(`${s}dds`)||(this.#q=!0),h().existsSync(`${s}etc2`)||(this.#A=!0),!this.#v&&!this.#M&&!this.#A&&this.#R>0){let t=null;for(let e of this.#p){let i=this.#k.getInfo(e.path);if(i&&i.etc){t=e;break}}if(!t)return;const e=t.path;let i=`${this.#D.getConfig("-outDir")}${y}etc2${y}${t.width}${y}`,s=`${i}${t.name}.txt`;if(!h().existsSync(s))return;let o=`${i}${t.name}.temp.txt`;this.#G(i);try{const i=new S(this.#O,e,o,t.width,this.#l);await i.convert();const n=h().readFileSync(s),c=h().readFileSync(o);let r=!0;for(let t=0;t<n.length;t++)if(t>=c.length||n[t]!=c[t]){r=!1;break}r||(this.#A=!0)}catch{}h().existsSync(o)&&h().rmSync(o)}}#E=!1;#q=!1;#A=!1;async#N(){for(;;){const t=this.#p.shift();if(!t)break;(new Date).getTime();const e=t.path,i=this.#D.getConfig("-outDir");let s=this.#k.getInfo(e);s||(s=this.#k.defaultStruct(e,t,this.#l)),this.#_(e);let o="",n="";if(this.#M||this.#E||!s.astc||s.colorSpace!=this.#l){(new Date).getTime(),o=`${i}${y}astc${y}${t.width}${y}`,n=`${o}${t.name}.txt`,this.#G(o);try{const i=new f(this.#O,e,n,t.astc,t.width,this.#l);await i.convert(),s.astc=!0}catch(t){this.#F++,l.ConsoleMessage(`Output path:[${n}] is failed.`)}}if(this.#v)this.#k.setInfo(s);else{if(this.#M||this.#q||!s.dxt||s.colorSpace!=this.#l){(new Date).getTime(),o=`${i}${y}dds${y}${t.width}${y}`,n=`${o}${t.name}.txt`,this.#G(o);try{const i=new d(this.#O,e,n,t.width,this.#l);await i.convert(),s.dxt=!0}catch(t){this.#F++,l.ConsoleMessage(`Output path:[${n}] is failed.`)}}if(this.#M||this.#A||!s.etc||s.colorSpace!=this.#l){(new Date).getTime(),o=`${i}${y}etc2${y}${t.width}${y}`,n=`${o}${t.name}.txt`,this.#G(o);try{const i=new S(this.#O,e,n,t.width,this.#l);await i.convert(),s.etc=!0}catch(t){this.#F++,l.ConsoleMessage(`Output path:[${n}] is failed.`)}}if(this.#M||!s.pngmin){(new Date).getTime();try{const i=new m(this.#O,e,e,t.width,this.#l);await i.convert(),s.pngmin=!0}catch(t){this.#F++,l.ConsoleMessage(`Output path:[${n}] is failed.`)}}if("Linear"==this.#l&&(this.#M||!s.lpngmin))try{o=`${i}${y}lpng${y}${t.width}${y}`,lpngPath=`${o}${t.name}.png`;const e=new m(this.#O,lpngPath,lpngPath,t.width,this.#l);await e.convert(),s.lpngmin=!0}catch(t){this.#F++,l.ConsoleMessage(`Output path:[${n}] is failed.`)}this.#k.setInfo(s)}}this.#b++,this.#X()}#_(t){l.ProgressMessage(this.#R-this.#p.length,this.#R,t)}#G(t){h().existsSync(t)||h().mkdirSync(t,{recursive:!0})}#W(){return new Promise(((t,e)=>{setTimeout((()=>{t()}),100)}))}#L=!1;#X(){this.#b==this.#T&&(this.#L=!0,this.#k.save(),l.ResponseMsg("All Asset Textures have been converted! (Failed count: "+this.#F+")"))}}(C).launch()})()})();