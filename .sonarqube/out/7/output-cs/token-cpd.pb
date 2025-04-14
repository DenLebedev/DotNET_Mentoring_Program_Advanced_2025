ÄW
PD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
c  
=>! #
{ 
c 
. 

SwaggerDoc 
( 
$str 
, 
new 
( 
) 
{ 
Title $
=% &
$str' 7
,7 8
Version9 @
=A B
$strC G
}H I
)I J
;J K
c 
. !
AddSecurityDefinition 
( 
$str $
,$ %
new& )
	Microsoft* 3
.3 4
OpenApi4 ;
.; <
Models< B
.B C!
OpenApiSecuritySchemeC X
{ 
Type 
= 
	Microsoft 
. 
OpenApi  
.  !
Models! '
.' (
SecuritySchemeType( :
.: ;
OAuth2; A
,A B
Flows 
= 
new 
	Microsoft 
. 
OpenApi %
.% &
Models& ,
., -
OpenApiOAuthFlows- >
{ 	
Password   
=   
new   
	Microsoft   $
.  $ %
OpenApi  % ,
.  , -
Models  - 3
.  3 4
OpenApiOAuthFlow  4 D
{!! 
TokenUrl"" 
="" 
new"" 
Uri"" "
(""" #
$str""# I
)""I J
,""J K
Scopes## 
=## 
new## 

Dictionary## '
<##' (
string##( .
,##. /
string##0 6
>##6 7
{$$ 
{%% 
$str%% #
,%%# $
$str%%% <
}%%= >
,%%> ?
{&& 
$str&& &
,&&& '
$str&&( 7
}&&8 9
,&&9 :
{'' 
$str'' 
,'' 
$str''  (
}'') *
,''* +
{(( 
$str(( 
,((  
$str((! *
}((+ ,
})) 
}** 
}++ 	
},, 
),, 
;,, 
c.. 
... "
AddSecurityRequirement.. 
(.. 
new..  
	Microsoft..! *
...* +
OpenApi..+ 2
...2 3
Models..3 9
...9 :&
OpenApiSecurityRequirement..: T
{// 
{00 	
new11 
	Microsoft11 
.11 
OpenApi11 !
.11! "
Models11" (
.11( )!
OpenApiSecurityScheme11) >
{22 
	Reference33 
=33 
new33 
	Microsoft33  )
.33) *
OpenApi33* 1
.331 2
Models332 8
.338 9
OpenApiReference339 I
{44 
Type55 
=55 
	Microsoft55 $
.55$ %
OpenApi55% ,
.55, -
Models55- 3
.553 4
ReferenceType554 A
.55A B
SecurityScheme55B P
,55P Q
Id66 
=66 
$str66 !
}77 
}88 
,88 
new99 
[99 
]99 
{99 
$str99 !
}99" #
}:: 	
};; 
);; 
;;; 
}<< 
)<< 
;<< 
builder?? 
.?? 
Services?? 
.?? 
AddAutoMapper?? 
(?? 
typeof?? %
(??% &!
CatalogMappingProfile??& ;
)??; <
)??< =
;??= >
builderBB 
.BB 
ServicesBB 
.BB  
AddDefaultAWSOptionsBB %
(BB% &
builderBB& -
.BB- .
ConfigurationBB. ;
.BB; <
GetAWSOptionsBB< I
(BBI J
)BBJ K
)BBK L
;BBL M
builderCC 
.CC 
ServicesCC 
.CC 
AddAWSServiceCC 
<CC 

IAmazonSQSCC )
>CC) *
(CC* +
)CC+ ,
;CC, -
builderDD 
.DD 
ServicesDD 
.DD 
	AddScopedDD 
<DD 
ISqsPublisherDD (
,DD( )
SqsPublisherDD* 6
>DD6 7
(DD7 8
)DD8 9
;DD9 :
builderGG 
.GG 
ServicesGG 
.GG 
	AddScopedGG 
<GG 
CategoryServiceGG *
>GG* +
(GG+ ,
)GG, -
;GG- .
builderHH 
.HH 
ServicesHH 
.HH 
	AddScopedHH 
<HH 
ProductServiceHH )
>HH) *
(HH* +
)HH+ ,
;HH, -
builderII 
.II 
ServicesII 
.II 
	AddScopedII 
<II 
ICategoryRepositoryII .
,II. /
CategoryRepositoryII0 B
>IIB C
(IIC D
)IID E
;IIE F
builderJJ 
.JJ 
ServicesJJ 
.JJ 
	AddScopedJJ 
<JJ 
IProductRepositoryJJ -
,JJ- .
ProductRepositoryJJ/ @
>JJ@ A
(JJA B
)JJB C
;JJC D
builderMM 
.MM 
ServicesMM 
.MM 
AddAuthenticationMM "
(MM" #
$strMM# +
)MM+ ,
.NN 
AddJwtBearerNN 
(NN 
$strNN 
,NN 
optionsNN #
=>NN$ &
{OO 
optionsPP 
.PP 
	AuthorityPP 
=PP 
$strPP 4
;PP4 5
optionsQQ 
.QQ  
RequireHttpsMetadataQQ $
=QQ% &
falseQQ' ,
;QQ, -
optionsRR 
.RR %
TokenValidationParametersRR )
=RR* +
newRR, /%
TokenValidationParametersRR0 I
{SS 	
ValidateAudienceTT 
=TT 
falseTT $
}UU 	
;UU	 

}VV 
)VV 
;VV 
builderXX 
.XX 
ServicesXX 
.XX 
AddAuthorizationXX !
(XX! "
optionsXX" )
=>XX* ,
{YY 
optionsZZ 
.ZZ 
	AddPolicyZZ 
(ZZ 
$strZZ #
,ZZ# $
policyZZ% +
=>ZZ, .
policyZZ/ 5
.ZZ5 6
RequireRoleZZ6 A
(ZZA B
$strZZB K
)ZZK L
)ZZL M
;ZZM N
}[[ 
)[[ 
;[[ 
builder^^ 
.^^ 
Services^^ 
.^^ 
AddSingleton^^ 
<^^ "
IActionContextAccessor^^ 4
,^^4 5!
ActionContextAccessor^^6 K
>^^K L
(^^L M
)^^M N
;^^N O
builder__ 
.__ 
Services__ 
.__ 
	AddScoped__ 
<__ 

IUrlHelper__ %
>__% &
(__& '
x__' (
=>__) +
{`` 
varaa !
actionContextAccessoraa 
=aa 
xaa  !
.aa! "
GetRequiredServiceaa" 4
<aa4 5"
IActionContextAccessoraa5 K
>aaK L
(aaL M
)aaM N
;aaN O
ifbb 
(bb !
actionContextAccessorbb 
.bb 
ActionContextbb +
==bb, .
nullbb/ 3
)bb3 4
{cc 
throwdd 
newdd %
InvalidOperationExceptiondd +
(dd+ ,
$strdd, K
)ddK L
;ddL M
}ee 
returnff 

xff 
.ff 
GetRequiredServiceff 
<ff  
IUrlHelperFactoryff  1
>ff1 2
(ff2 3
)ff3 4
.ff4 5
GetUrlHelperff5 A
(ffA B!
actionContextAccessorffB W
.ffW X
ActionContextffX e
)ffe f
;fff g
}gg 
)gg 
;gg 
builderii 
.ii 
Servicesii 
.ii 
AddDbContextii 
<ii 
CatalogDbContextii .
>ii. /
(ii/ 0
optionsii0 7
=>ii8 :
optionsjj 
.jj 
UseMySqljj 
(jj 
builderkk 
.kk 
Configurationkk 
.kk 
GetConnectionStringkk 1
(kk1 2
$strkk2 E
)kkE F
,kkF G
newll 
MySqlServerVersionll 
(ll 
newll "
Versionll# *
(ll* +
$numll+ ,
,ll, -
$numll. /
,ll/ 0
$numll1 3
)ll3 4
)ll4 5
)mm 
)nn 
;nn 
varpp 
apppp 
=pp 	
builderpp
 
.pp 
Buildpp 
(pp 
)pp 
;pp 
ifrr 
(rr 
apprr 
.rr 
Environmentrr 
.rr 
IsDevelopmentrr !
(rr! "
)rr" #
)rr# $
{ss 
apptt 
.tt 

UseSwaggertt 
(tt 
)tt 
;tt 
appuu 
.uu 
UseSwaggerUIuu 
(uu 
cuu 
=>uu 
{vv 
cww 	
.ww	 

SwaggerEndpointww
 
(ww 
$strww 4
,ww4 5
$strww6 I
)wwI J
;wwJ K
cyy 	
.yy	 

OAuthClientIdyy
 
(yy 
$stryy $
)yy$ %
;yy% &
czz 	
.zz	 

OAuthClientSecretzz
 
(zz 
$strzz ,
)zz, -
;zz- .
c{{ 	
.{{	 

OAuthAppName{{
 
({{ 
$str{{ /
){{/ 0
;{{0 1
c|| 	
.||	 

OAuthUsePkce||
 
(|| 
)|| 
;|| 
}}} 
)}} 
;}} 
}~~ 
appÄÄ 
.
ÄÄ !
UseHttpsRedirection
ÄÄ 
(
ÄÄ 
)
ÄÄ 
;
ÄÄ 
appÅÅ 
.
ÅÅ 
UseAuthentication
ÅÅ 
(
ÅÅ 
)
ÅÅ 
;
ÅÅ 
appÇÇ 
.
ÇÇ 
UseAuthorization
ÇÇ 
(
ÇÇ 
)
ÇÇ 
;
ÇÇ 
appÉÉ 
.
ÉÉ 
MapControllers
ÉÉ 
(
ÉÉ 
)
ÉÉ 
;
ÉÉ 
appÑÑ 
.
ÑÑ 
Run
ÑÑ 
(
ÑÑ 
)
ÑÑ 	
;
ÑÑ	 

publicÜÜ 
partial
ÜÜ 
class
ÜÜ 
Program
ÜÜ 
{
ÜÜ 
}
ÜÜ  ®M
gD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService\Controllers\ProductsController.cs
	namespace 	
CatalogService
 
. 
Controllers $
{		 
[

 
ApiController

 
]

 
[ 
Route 

(
 
$str 
) 
] 
public 

class 
ProductsController #
:$ %
ControllerBase& 4
{ 
private 
readonly 
ProductService '
_productService( 7
;7 8
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 

IUrlHelper #

_urlHelper$ .
;. /
public 
ProductsController !
(! "
ProductService" 0
productService1 ?
,? @
IMapperA H
mapperI O
,O P

IUrlHelperQ [
	urlHelper\ e
)e f
{ 	
_productService 
= 
productService ,
;, -
_mapper 
= 
mapper 
; 

_urlHelper 
= 
	urlHelper "
;" #
} 	
[ 	
HttpGet	 
] 
[ 	
AllowAnonymous	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
[- .
	FromQuery. 7
]7 8
int9 <
?< =

categoryId> H
,H I
[J K
	FromQueryK T
]T U
intV Y
pageZ ^
=_ `
$numa b
,b c
[d e
	FromQuerye n
]n o
intp s
pageSizet |
=} ~
$num	 Å
)
Å Ç
{ 	
var 
products 
= 
await  
_productService! 0
.0 1
GetProductsAsync1 A
(A B

categoryIdB L
,L M
pageN R
,R S
pageSizeT \
)\ ]
;] ^
var 
productDtos 
= 
_mapper %
.% &
Map& )
<) *
IEnumerable* 5
<5 6

ProductDto6 @
>@ A
>A B
(B C
productsC K
)K L
;L M
foreach   
(   
var   

productDto   #
in  $ &
productDtos  ' 2
)  2 3
{!! !
CreateLinksForProduct"" %
(""% &

productDto""& 0
)""0 1
;""1 2
}## 
return%% 
Ok%% 
(%% 
productDtos%% !
)%%! "
;%%" #
}&& 	
[(( 	
HttpGet((	 
((( 
$str(( 
,(( 
Name(( 
=(( 
$str((  0
)((0 1
]((1 2
[)) 	
AllowAnonymous))	 
])) 
public** 
async** 
Task** 
<** 
IActionResult** '
>**' (
Get**) ,
(**, -
int**- 0
id**1 3
)**3 4
{++ 	
var,, 
product,, 
=,, 
await,, 
_productService,,  /
.,,/ 0
GetByIdAsync,,0 <
(,,< =
id,,= ?
),,? @
;,,@ A
if-- 
(-- 
product-- 
==-- 
null-- 
)--  
return.. 
NotFound.. 
(..  
)..  !
;..! "
var00 

productDto00 
=00 
_mapper00 $
.00$ %
Map00% (
<00( )

ProductDto00) 3
>003 4
(004 5
product005 <
)00< =
;00= >!
CreateLinksForProduct11 !
(11! "

productDto11" ,
)11, -
;11- .
return33 
Ok33 
(33 

productDto33  
)33  !
;33! "
}44 	
[66 	
HttpPost66	 
]66 
[77 	
	Authorize77	 
(77 
Roles77 
=77 
$str77 $
)77$ %
]77% &
public88 
async88 
Task88 
<88 
IActionResult88 '
>88' (
Post88) -
(88- .
[88. /
FromBody88/ 7
]887 8
CreateProductDto889 I
	createDto88J S
)88S T
{99 	
var:: 
createdProduct:: 
=::  
await::! &
_productService::' 6
.::6 7
AddAsync::7 ?
(::? @
	createDto::@ I
)::I J
;::J K
var;; 
result;; 
=;; 
_mapper;;  
.;;  !
Map;;! $
<;;$ %

ProductDto;;% /
>;;/ 0
(;;0 1
createdProduct;;1 ?
);;? @
;;;@ A!
CreateLinksForProduct<< !
(<<! "
result<<" (
)<<( )
;<<) *
return== 
CreatedAtAction== "
(==" #
nameof==# )
(==) *
Get==* -
)==- .
,==. /
new==0 3
{==4 5
id==6 8
===9 :
result==; A
.==A B
Id==B D
}==E F
,==F G
result==H N
)==N O
;==O P
}>> 	
[@@ 	
HttpPut@@	 
(@@ 
$str@@ 
,@@ 
Name@@ 
=@@ 
$str@@  /
)@@/ 0
]@@0 1
[AA 	
	AuthorizeAA	 
(AA 
RolesAA 
=AA 
$strAA $
)AA$ %
]AA% &
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
PutBB) ,
(BB, -
intBB- 0
idBB1 3
,BB3 4
[BB5 6
FromBodyBB6 >
]BB> ?
UpdateProductDtoBB@ P
	updateDtoBBQ Z
)BBZ [
{CC 	
ifDD 
(DD 
idDD 
!=DD 
	updateDtoDD 
.DD  
IdDD  "
)DD" #
returnEE 

BadRequestEE !
(EE! "
)EE" #
;EE# $
varGG 
updatedProductGG 
=GG  
awaitGG! &
_productServiceGG' 6
.GG6 7
UpdateAsyncGG7 B
(GGB C
idGGC E
,GGE F
	updateDtoGGG P
)GGP Q
;GGQ R
ifHH 
(HH 
updatedProductHH 
==HH !
nullHH" &
)HH& '
returnII 
NotFoundII 
(II  
)II  !
;II! "
varKK 
resultKK 
=KK 
_mapperKK  
.KK  !
MapKK! $
<KK$ %

ProductDtoKK% /
>KK/ 0
(KK0 1
updatedProductKK1 ?
)KK? @
;KK@ A!
CreateLinksForProductLL !
(LL! "
resultLL" (
)LL( )
;LL) *
returnMM 
OkMM 
(MM 
resultMM 
)MM 
;MM 
}NN 	
[PP 	

HttpDeletePP	 
(PP 
$strPP 
,PP 
NamePP  
=PP! "
$strPP# 2
)PP2 3
]PP3 4
[QQ 	
	AuthorizeQQ	 
(QQ 
RolesQQ 
=QQ 
$strQQ $
)QQ$ %
]QQ% &
publicRR 
asyncRR 
TaskRR 
<RR 
IActionResultRR '
>RR' (
DeleteRR) /
(RR/ 0
intRR0 3
idRR4 6
)RR6 7
{SS 	
awaitTT 
_productServiceTT !
.TT! "
DeleteAsyncTT" -
(TT- .
idTT. 0
)TT0 1
;TT1 2
returnUU 
	NoContentUU 
(UU 
)UU 
;UU 
}VV 	
privateWW 
voidWW !
CreateLinksForProductWW *
(WW* +

ProductDtoWW+ 5

productDtoWW6 @
)WW@ A
{XX 	

productDtoYY 
.YY 
LinksYY 
.YY 
AddYY  
(YY  !
newYY! $
LinkYY% )
(YY) *

_urlHelperYY* 4
.YY4 5
LinkYY5 9
(YY9 :
$strYY: J
,YYJ K
newYYL O
{YYP Q
idYYR T
=YYU V

productDtoYYW a
.YYa b
IdYYb d
}YYe f
)YYf g
,YYg h
$strYYi o
,YYo p
$strYYq v
)YYv w
)YYw x
;YYx y

productDtoZZ 
.ZZ 
LinksZZ 
.ZZ 
AddZZ  
(ZZ  !
newZZ! $
LinkZZ% )
(ZZ) *

_urlHelperZZ* 4
.ZZ4 5
LinkZZ5 9
(ZZ9 :
$strZZ: I
,ZZI J
newZZK N
{ZZO P
idZZQ S
=ZZT U

productDtoZZV `
.ZZ` a
IdZZa c
}ZZd e
)ZZe f
,ZZf g
$strZZh x
,ZZx y
$strZZz 
)	ZZ Ä
)
ZZÄ Å
;
ZZÅ Ç

productDto[[ 
.[[ 
Links[[ 
.[[ 
Add[[  
([[  !
new[[! $
Link[[% )
([[) *

_urlHelper[[* 4
.[[4 5
Link[[5 9
([[9 :
$str[[: I
,[[I J
new[[K N
{[[O P
id[[Q S
=[[T U

productDto[[V `
.[[` a
Id[[a c
}[[d e
)[[e f
,[[f g
$str[[h x
,[[x y
$str	[[z Ç
)
[[Ç É
)
[[É Ñ
;
[[Ñ Ö
}\\ 	
}]] 
}^^ ⁄R
iD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService\Controllers\CategoriesController.cs
	namespace 	
CatalogService
 
. 
Controllers $
{		 
[

 
ApiController

 
]

 
[ 
Route 

(
 
$str 
) 
] 
public 

class  
CategoriesController %
:& '
ControllerBase( 6
{ 
private 
readonly 
CategoryService (
_categoryService) 9
;9 :
private 
readonly 
IMapper  
_mapper! (
;( )
private 
readonly 

IUrlHelper #

_urlHelper$ .
;. /
public  
CategoriesController #
(# $
CategoryService$ 3
categoryService4 C
,C D
IMapperE L
mapperM S
,S T

IUrlHelperU _
	urlHelper` i
)i j
{ 	
_categoryService 
= 
categoryService .
;. /
_mapper 
= 
mapper 
; 

_urlHelper 
= 
	urlHelper "
;" #
} 	
[ 	
HttpGet	 
] 
[ 	
AllowAnonymous	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Get) ,
(, -
)- .
{ 	
var 

categories 
= 
await "
_categoryService# 3
.3 4
GetAllAsync4 ?
(? @
)@ A
;A B
var 
categoryDtos 
= 
_mapper &
.& '
Map' *
<* +
IEnumerable+ 6
<6 7
CategoryDto7 B
>B C
>C D
(D E

categoriesE O
)O P
;P Q
foreach   
(   
var   
categoryDto   $
in  % '
categoryDtos  ( 4
)  4 5
{!! "
CreateLinksForCategory"" &
(""& '
categoryDto""' 2
)""2 3
;""3 4
}## 
return%% 
Ok%% 
(%% 
categoryDtos%% "
)%%" #
;%%# $
}&& 	
[(( 	
HttpGet((	 
((( 
$str(( 
,(( 
Name(( 
=(( 
$str((  1
)((1 2
]((2 3
[)) 	
AllowAnonymous))	 
])) 
public** 
async** 
Task** 
<** 
IActionResult** '
>**' (
Get**) ,
(**, -
int**- 0
id**1 3
)**3 4
{++ 	
var,, 
category,, 
=,, 
await,,  
_categoryService,,! 1
.,,1 2
GetByIdAsync,,2 >
(,,> ?
id,,? A
),,A B
;,,B C
if-- 
(-- 
category-- 
==-- 
null--  
)--  !
return.. 
NotFound.. 
(..  
)..  !
;..! "
var00 
categoryDto00 
=00 
_mapper00 %
.00% &
Map00& )
<00) *
CategoryDto00* 5
>005 6
(006 7
category007 ?
)00? @
;00@ A"
CreateLinksForCategory11 "
(11" #
categoryDto11# .
)11. /
;11/ 0
return33 
Ok33 
(33 
categoryDto33 !
)33! "
;33" #
}44 	
[66 	
HttpPost66	 
]66 
[77 	
	Authorize77	 
(77 
Roles77 
=77 
$str77 $
)77$ %
]77% &
public88 
async88 
Task88 
<88 
IActionResult88 '
>88' (
Post88) -
(88- .
[88. /
FromBody88/ 7
]887 8
CreateCategoryDto889 J
	createDto88K T
)88T U
{99 	
var:: 
createdCategory:: 
=::  !
await::" '
_categoryService::( 8
.::8 9
AddAsync::9 A
(::A B
	createDto::B K
)::K L
;::L M
var;; 
result;; 
=;; 
_mapper;;  
.;;  !
Map;;! $
<;;$ %
CategoryDto;;% 0
>;;0 1
(;;1 2
createdCategory;;2 A
);;A B
;;;B C"
CreateLinksForCategory<< "
(<<" #
result<<# )
)<<) *
;<<* +
return== 
CreatedAtAction== "
(==" #
nameof==# )
(==) *
Get==* -
)==- .
,==. /
new==0 3
{==4 5
id==6 8
===9 :
result==; A
.==A B
Id==B D
}==E F
,==F G
result==H N
)==N O
;==O P
}>> 	
[@@ 	
HttpPut@@	 
(@@ 
$str@@ 
,@@ 
Name@@ 
=@@ 
$str@@  0
)@@0 1
]@@1 2
[AA 	
	AuthorizeAA	 
(AA 
RolesAA 
=AA 
$strAA $
)AA$ %
]AA% &
publicBB 
asyncBB 
TaskBB 
<BB 
IActionResultBB '
>BB' (
PutBB) ,
(BB, -
intBB- 0
idBB1 3
,BB3 4
[BB5 6
FromBodyBB6 >
]BB> ?
UpdateCategoryDtoBB@ Q
	updateDtoBBR [
)BB[ \
{CC 	
ifDD 
(DD 
idDD 
!=DD 
	updateDtoDD 
.DD  
IdDD  "
)DD" #
returnEE 

BadRequestEE !
(EE! "
)EE" #
;EE# $
varGG 
updatedCategoryGG 
=GG  !
awaitGG" '
_categoryServiceGG( 8
.GG8 9
UpdateAsyncGG9 D
(GGD E
idGGE G
,GGG H
	updateDtoGGI R
)GGR S
;GGS T
ifHH 
(HH 
updatedCategoryHH 
==HH  "
nullHH# '
)HH' (
returnII 
NotFoundII 
(II  
)II  !
;II! "
varKK 
resultKK 
=KK 
_mapperKK  
.KK  !
MapKK! $
<KK$ %
CategoryDtoKK% 0
>KK0 1
(KK1 2
updatedCategoryKK2 A
)KKA B
;KKB C"
CreateLinksForCategoryLL "
(LL" #
resultLL# )
)LL) *
;LL* +
returnMM 
OkMM 
(MM 
resultMM 
)MM 
;MM 
}NN 	
[PP 	

HttpDeletePP	 
(PP 
$strPP 
,PP 
NamePP  
=PP! "
$strPP# 3
)PP3 4
]PP4 5
[QQ 	
	AuthorizeQQ	 
(QQ 
RolesQQ 
=QQ 
$strQQ $
)QQ$ %
]QQ% &
publicRR 
asyncRR 
TaskRR 
<RR 
IActionResultRR '
>RR' (
DeleteRR) /
(RR/ 0
intRR0 3
idRR4 6
)RR6 7
{SS 	
awaitTT 
_categoryServiceTT "
.TT" #
DeleteAsyncTT# .
(TT. /
idTT/ 1
)TT1 2
;TT2 3
returnUU 
	NoContentUU 
(UU 
)UU 
;UU 
}VV 	
[XX 	

HttpDeleteXX	 
(XX 
$strXX (
,XX( )
NameXX* .
=XX/ 0
$strXX1 M
)XXM N
]XXN O
[YY 	
	AuthorizeYY	 
(YY 
RolesYY 
=YY 
$strYY $
)YY$ %
]YY% &
publicZZ 
asyncZZ 
TaskZZ 
<ZZ 
IActionResultZZ '
>ZZ' (%
DeleteCateoryWithProductsZZ) B
(ZZB C
intZZC F
idZZG I
)ZZI J
{[[ 	
try\\ 
{]] 
await^^ 
_categoryService^^ &
.^^& '+
DeleteCategoryWithProductsAsync^^' F
(^^F G
id^^G I
)^^I J
;^^J K
return__ 
	NoContent__  
(__  !
)__! "
;__" #
}`` 
catchaa 
(aa 
ArgumentExceptionaa $
exaa% '
)aa' (
{bb 
returncc 
NotFoundcc 
(cc  
excc  "
.cc" #
Messagecc# *
)cc* +
;cc+ ,
}dd 
}ee 	
privategg 
voidgg "
CreateLinksForCategorygg +
(gg+ ,
CategoryDtogg, 7
categoryDtogg8 C
)ggC D
{hh 	
categoryDtoii 
.ii 
Linksii 
.ii 
Addii !
(ii! "
newii" %
Linkii& *
(ii* +

_urlHelperii+ 5
.ii5 6
Linkii6 :
(ii: ;
$strii; L
,iiL M
newiiN Q
{iiR S
idiiT V
=iiW X
categoryDtoiiY d
.iid e
Idiie g
}iih i
)iii j
,iij k
$striil r
,iir s
$striit y
)iiy z
)iiz {
;ii{ |
categoryDtojj 
.jj 
Linksjj 
.jj 
Addjj !
(jj! "
newjj" %
Linkjj& *
(jj* +

_urlHelperjj+ 5
.jj5 6
Linkjj6 :
(jj: ;
$strjj; K
,jjK L
newjjM P
{jjQ R
idjjS U
=jjV W
categoryDtojjX c
.jjc d
Idjjd f
}jjg h
)jjh i
,jji j
$strjjk |
,jj| }
$str	jj~ É
)
jjÉ Ñ
)
jjÑ Ö
;
jjÖ Ü
categoryDtokk 
.kk 
Linkskk 
.kk 
Addkk !
(kk! "
newkk" %
Linkkk& *
(kk* +

_urlHelperkk+ 5
.kk5 6
Linkkk6 :
(kk: ;
$strkk; K
,kkK L
newkkM P
{kkQ R
idkkS U
=kkV W
categoryDtokkX c
.kkc d
Idkkd f
}kkg h
)kkh i
,kki j
$strkkk |
,kk| }
$str	kk~ Ü
)
kkÜ á
)
kká à
;
kkà â
}ll 	
}mm 
}nn 