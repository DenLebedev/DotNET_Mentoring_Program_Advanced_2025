Ž=
lD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\Services\ProductService.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
Services% -
;- .
public		 
class		 
ProductService		 
:		 
IProductService		 -
{

 
private 
readonly 
IProductRepository '
_repository( 3
;3 4
private 
readonly 
IMapper 
_mapper $
;$ %
private 
readonly 
ISqsPublisher "

_publisher# -
;- .
public 

ProductService 
( 
IProductRepository ,

repository- 7
,7 8
IMapper9 @
mapperA G
,G H
ISqsPublisherI V
	publisherW `
)` a
{ 
_repository 
= 

repository  
;  !
_mapper 
= 
mapper 
; 

_publisher 
= 
	publisher 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! "

ProductDto" ,
>, -
>- .
GetProductsAsync/ ?
(? @
int@ C
?C D

categoryIdE O
,O P
intQ T
pageU Y
,Y Z
int[ ^
pageSize_ g
)g h
{ 
var 
products 
= 
await 
_repository (
.( )
GetProductsAsync) 9
(9 :

categoryId: D
,D E
pageF J
,J K
pageSizeL T
)T U
;U V
return 
_mapper 
. 
Map 
< 
IEnumerable &
<& '

ProductDto' 1
>1 2
>2 3
(3 4
products4 <
)< =
;= >
} 
public 

async 
Task 
< 

ProductDto  
?  !
>! "
GetByIdAsync# /
(/ 0
int0 3
id4 6
)6 7
{ 
var 
product 
= 
await 
_repository '
.' (
GetByIdAsync( 4
(4 5
id5 7
)7 8
;8 9
return 
_mapper 
. 
Map 
< 

ProductDto %
?% &
>& '
(' (
product( /
)/ 0
;0 1
}   
public"" 

async"" 
Task"" 
<"" 

ProductDto""  
>""  !
AddAsync""" *
(""* +
CreateProductDto""+ ;

productDto""< F
)""F G
{## 
if$$ 

($$ 
string$$ 
.$$ 
IsNullOrWhiteSpace$$ %
($$% &

productDto$$& 0
.$$0 1
Name$$1 5
)$$5 6
||$$7 9

productDto$$: D
.$$D E
Name$$E I
.$$I J
Length$$J P
>$$Q R
$num$$S U
)$$U V
throw%% 
new%% 
ArgumentException%% '
(%%' (
$str%%( k
)%%k l
;%%l m
if&& 

(&& 

productDto&& 
.&& 
Amount&& 
<&& 
$num&&  !
)&&! "
throw'' 
new'' 
ArgumentException'' '
(''' (
$str''( B
)''B C
;''C D
if(( 

((( 

productDto(( 
.(( 
Price(( 
<(( 
$num((  
)((  !
throw)) 
new)) 
ArgumentException)) '
())' (
$str))( A
)))A B
;))B C
var++ 
product++ 
=++ 
_mapper++ 
.++ 
Map++ !
<++! "
Product++" )
>++) *
(++* +

productDto+++ 5
)++5 6
;++6 7
await,, 
_repository,, 
.,, 
AddAsync,, "
(,," #
product,,# *
),,* +
;,,+ ,
return-- 
_mapper-- 
.-- 
Map-- 
<-- 

ProductDto-- %
>--% &
(--& '
product--' .
)--. /
;--/ 0
}.. 
public00 

async00 
Task00 
<00 

ProductDto00  
?00  !
>00! "
UpdateAsync00# .
(00. /
int00/ 2
id003 5
,005 6
UpdateProductDto007 G

productDto00H R
)00R S
{11 
var22 
product22 
=22 
await22 
_repository22 '
.22' (
GetByIdAsync22( 4
(224 5
id225 7
)227 8
;228 9
if33 

(33 
product33 
==33 
null33 
)33 
{44 	
return55 
null55 
;55 
}66 	
else77 
{88 	
if99 
(99 
string99 
.99 
IsNullOrWhiteSpace99 )
(99) *

productDto99* 4
.994 5
Name995 9
)999 :
||99; =

productDto99> H
.99H I
Name99I M
.99M N
Length99N T
>99U V
$num99W Y
)99Y Z
throw:: 
new:: 
ArgumentException:: +
(::+ ,
$str::, o
)::o p
;::p q
if;; 
(;; 

productDto;; 
.;; 
Amount;; !
<;;" #
$num;;$ %
);;% &
throw<< 
new<< 
ArgumentException<< +
(<<+ ,
$str<<, F
)<<F G
;<<G H
if== 
(== 

productDto== 
.== 
Price==  
<==! "
$num==# $
)==$ %
throw>> 
new>> 
ArgumentException>> +
(>>+ ,
$str>>, E
)>>E F
;>>F G
}?? 	
_mapperAA 
.AA 
MapAA 
(AA 

productDtoAA 
,AA 
productAA  '
)AA' (
;AA( )
awaitBB 
_repositoryBB 
.BB 
UpdateAsyncBB %
(BB% &
productBB& -
)BB- .
;BB. /
varEE 
messageEE 
=EE 
newEE %
CatalogItemUpdatedMessageEE 3
{FF 	
IdGG 
=GG 
productGG 
.GG 
IdGG 
,GG 
NameHH 
=HH 
productHH 
.HH 
NameHH 
,HH  
PriceII 
=II 
productII 
.II 
PriceII !
}JJ 	
;JJ	 

awaitLL 

_publisherLL 
.LL *
PublishCatalogItemUpdatedAsyncLL 7
(LL7 8
messageLL8 ?
)LL? @
;LL@ A
returnNN 
_mapperNN 
.NN 
MapNN 
<NN 

ProductDtoNN %
>NN% &
(NN& '
productNN' .
)NN. /
;NN/ 0
}OO 
publicQQ 

asyncQQ 
TaskQQ 
DeleteAsyncQQ !
(QQ! "
intQQ" %
idQQ& (
)QQ( )
{RR 
awaitSS 
_repositorySS 
.SS 
DeleteAsyncSS %
(SS% &
idSS& (
)SS( )
;SS) *
}TT 
}UU Ê:
mD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\Services\CategoryService.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
Services% -
;- .
public		 
class		 
CategoryService		 
:		 
ICategoryService		 /
{

 
private 
readonly 
ICategoryRepository (
_categoryRepository) <
;< =
private 
readonly 
IProductRepository '
_productRepository( :
;: ;
private 
readonly 
IMapper 
_mapper $
;$ %
public 

CategoryService 
( 
ICategoryRepository .

repository/ 9
,9 :
IProductRepository; M
productRepositoryN _
,_ `
IMappera h
mapperi o
)o p
{ 
_categoryRepository 
= 

repository (
;( )
_productRepository 
= 
productRepository .
;. /
_mapper 
= 
mapper 
; 
} 
public 

async 
Task 
< 
IEnumerable !
<! "
CategoryDto" -
>- .
>. /
GetAllAsync0 ;
(; <
)< =
{ 
var 

categories 
= 
await 
_categoryRepository 2
.2 3
GetAllAsync3 >
(> ?
)? @
;@ A
return 
_mapper 
. 
Map 
< 
IEnumerable &
<& '
CategoryDto' 2
>2 3
>3 4
(4 5

categories5 ?
)? @
;@ A
} 
public 

async 
Task 
< 
CategoryDto !
?! "
>" #
GetByIdAsync$ 0
(0 1
int1 4
id5 7
)7 8
{ 
var 
category 
= 
await 
_categoryRepository 0
.0 1
GetByIdAsync1 =
(= >
id> @
)@ A
;A B
return 
_mapper 
. 
Map 
< 
CategoryDto &
?& '
>' (
(( )
category) 1
)1 2
;2 3
}   
public"" 

async"" 
Task"" 
<"" 
CategoryDto"" !
>""! "
AddAsync""# +
(""+ ,
CreateCategoryDto"", =
categoryDto""> I
)""I J
{## 
if$$ 

($$ 
string$$ 
.$$ 
IsNullOrWhiteSpace$$ %
($$% &
categoryDto$$& 1
.$$1 2
Name$$2 6
)$$6 7
||$$8 :
categoryDto$$; F
.$$F G
Name$$G K
.$$K L
Length$$L R
>$$S T
$num$$U W
)$$W X
throw%% 
new%% 
ArgumentException%% '
(%%' (
$str%%( k
)%%k l
;%%l m
var'' 
category'' 
='' 
_mapper'' 
.'' 
Map'' "
<''" #
Category''# +
>''+ ,
('', -
categoryDto''- 8
)''8 9
;''9 :
await(( 
_categoryRepository(( !
.((! "
AddAsync((" *
(((* +
category((+ 3
)((3 4
;((4 5
return)) 
_mapper)) 
.)) 
Map)) 
<)) 
CategoryDto)) &
>))& '
())' (
category))( 0
)))0 1
;))1 2
}** 
public,, 

async,, 
Task,, 
<,, 
CategoryDto,, !
?,,! "
>,," #
UpdateAsync,,$ /
(,,/ 0
int,,0 3
id,,4 6
,,,6 7
UpdateCategoryDto,,8 I
categoryDto,,J U
),,U V
{-- 
var.. 
category.. 
=.. 
await.. 
_categoryRepository.. 0
...0 1
GetByIdAsync..1 =
(..= >
id..> @
)..@ A
;..A B
if// 

(// 
category// 
==// 
null// 
)// 
{00 	
return11 
null11 
;11 
}22 	
else33 
{44 	
if55 
(55 
string55 
.55 
IsNullOrWhiteSpace55 )
(55) *
categoryDto55* 5
.555 6
Name556 :
)55: ;
||55< >
categoryDto55? J
.55J K
Name55K O
.55O P
Length55P V
>55W X
$num55Y [
)55[ \
throw66 
new66 
ArgumentException66 +
(66+ ,
$str66, o
)66o p
;66p q
}77 	
_mapper99 
.99 
Map99 
(99 
categoryDto99 
,99  
category99! )
)99) *
;99* +
await:: 
_categoryRepository:: !
.::! "
UpdateAsync::" -
(::- .
category::. 6
)::6 7
;::7 8
return;; 
_mapper;; 
.;; 
Map;; 
<;; 
CategoryDto;; &
>;;& '
(;;' (
category;;( 0
);;0 1
;;;1 2
}<< 
public>> 

async>> 
Task>> 
DeleteAsync>> !
(>>! "
int>>" %
id>>& (
)>>( )
{?? 
await@@ 
_categoryRepository@@ !
.@@! "
DeleteAsync@@" -
(@@- .
id@@. 0
)@@0 1
;@@1 2
}AA 
publicCC 

asyncCC 
TaskCC +
DeleteCategoryWithProductsAsyncCC 5
(CC5 6
intCC6 9
idCC: <
)CC< =
{DD 
varEE 
categoryEE 
=EE 
awaitEE 
_categoryRepositoryEE 0
.EE0 1
GetByIdAsyncEE1 =
(EE= >
idEE> @
)EE@ A
;EEA B
ifFF 

(FF 
categoryFF 
==FF 
nullFF 
)FF 
{GG 	
throwHH 
newHH 
ArgumentExceptionHH '
(HH' (
$strHH( =
)HH= >
;HH> ?
}II 	
varLL 
productsLL 
=LL 
awaitLL 
_productRepositoryLL /
.LL/ 0
GetProductsAsyncLL0 @
(LL@ A
idLLA C
,LLC D
$numLLE F
,LLF G
intLLH K
.LLK L
MaxValueLLL T
)LLT U
;LLU V
foreachMM 
(MM 
varMM 
productMM 
inMM 
productsMM  (
)MM( )
{NN 	
awaitOO 
_productRepositoryOO $
.OO$ %
DeleteAsyncOO% 0
(OO0 1
productOO1 8
.OO8 9
IdOO9 ;
)OO; <
;OO< =
}PP 	
awaitSS 
_categoryRepositorySS !
.SS! "
DeleteAsyncSS" -
(SS- .
idSS. 0
)SS0 1
;SS1 2
}TT 
}UU —
sD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\Mappings\CatalogMappingProfile.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
Mappings% -
;- .
public 
class !
CatalogMappingProfile "
:# $
Profile% ,
{		 
public

 
!
CatalogMappingProfile

  
(

  !
)

! "
{ 
	CreateMap 
< 
Category 
, 
CategoryDto '
>' (
(( )
)) *
;* +
	CreateMap 
< 
CreateCategoryDto #
,# $
Category% -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
UpdateCategoryDto #
,# $
Category% -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
Category 
, 
UpdateCategoryDto -
>- .
(. /
)/ 0
;0 1
	CreateMap 
< 
Product 
, 

ProductDto %
>% &
(& '
)' (
;( )
	CreateMap 
< 
CreateProductDto "
," #
Product$ +
>+ ,
(, -
)- .
;. /
	CreateMap 
< 
UpdateProductDto "
," #
Product$ +
>+ ,
(, -
)- .
;. /
	CreateMap 
< 
Product 
, 
UpdateProductDto +
>+ ,
(, -
)- .
;. /
} 
} ’
lD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\Intefaces\ISqsPublisher.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
	Intefaces% .
{ 
public 

	interface 
ISqsPublisher "
{ 
Task *
PublishCatalogItemUpdatedAsync +
(+ ,%
CatalogItemUpdatedMessage, E
messageF M
)M N
;N O
} 
} Õ
nD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\Intefaces\IProductService.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
	Intefaces% .
{ 
public 

	interface 
IProductService $
{ 
Task 
< 
IEnumerable 
< 

ProductDto #
># $
>$ %
GetProductsAsync& 6
(6 7
int7 :
?: ;

categoryId< F
,F G
intH K
pageL P
,P Q
intR U
pageSizeV ^
)^ _
;_ `
Task 
< 

ProductDto 
? 
> 
GetByIdAsync &
(& '
int' *
id+ -
)- .
;. /
Task		 
<		 

ProductDto		 
>		 
AddAsync		 !
(		! "
CreateProductDto		" 2

productDto		3 =
)		= >
;		> ?
Task

 
<

 

ProductDto

 
?

 
>

 
UpdateAsync

 %
(

% &
int

& )
id

* ,
,

, -
UpdateProductDto

. >

productDto

? I
)

I J
;

J K
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
} 
} Ç
oD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\Intefaces\ICategoryService.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
	Intefaces% .
{ 
public 

	interface 
ICategoryService %
{ 
Task 
< 
IEnumerable 
< 
CategoryDto $
>$ %
>% &
GetAllAsync' 2
(2 3
)3 4
;4 5
Task 
< 
CategoryDto 
? 
> 
GetByIdAsync '
(' (
int( +
id, .
). /
;/ 0
Task		 
<		 
CategoryDto		 
>		 
AddAsync		 "
(		" #
CreateCategoryDto		# 4
categoryDto		5 @
)		@ A
;		A B
Task

 
<

 
CategoryDto

 
?

 
>

 
UpdateAsync

 &
(

& '
int

' *
id

+ -
,

- .
UpdateCategoryDto

/ @
categoryDto

A L
)

L M
;

M N
Task 
DeleteAsync 
( 
int 
id 
)  
;  !
Task +
DeleteCategoryWithProductsAsync ,
(, -
int- 0
id1 3
)3 4
;4 5
} 
} ä
jD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\UpdateProductDto.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
DTOs% )
;) *
public 
class 
UpdateProductDto 
{ 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
=% &
default' .
!. /
;/ 0
public 

string 
? 
Description 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 

string 
? 
ImageUrl 
{ 
get !
;! "
set# &
;& '
}( )
public		 

decimal		 
Price		 
{		 
get		 
;		 
set		  #
;		# $
}		% &
public

 

int

 
Amount

 
{

 
get

 
;

 
set

  
;

  !
}

" #
public 

int 

CategoryId 
{ 
get 
;  
set! $
;$ %
}& '
} œ
kD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\UpdateCategoryDto.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
DTOs% )
;) *
public 
class 
UpdateCategoryDto 
{ 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
=% &
default' .
!. /
;/ 0
public 

string 
? 
ImageUrl 
{ 
get !
;! "
set# &
;& '
}( )
public 

int 
? 
ParentCategoryId  
{! "
get# &
;& '
set( +
;+ ,
}- .
}		 ®
dD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\ProductDto.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
DTOs% )
;) *
public 
class 

ProductDto 
{ 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
=% &
default' .
!. /
;/ 0
public		 

string		 
?		 
Description		 
{		  
get		! $
;		$ %
set		& )
;		) *
}		+ ,
public

 

string

 
?

 
ImageUrl

 
{

 
get

 !
;

! "
set

# &
;

& '
}

( )
public 

decimal 
Price 
{ 
get 
; 
set  #
;# $
}% &
public 

int 
Amount 
{ 
get 
; 
set  
;  !
}" #
public 

int 

CategoryId 
{ 
get 
;  
set! $
;$ %
}& '
public 

List 
< 
Link 
> 
Links 
{ 
get !
;! "
set# &
;& '
}( )
=* +
new, /
List0 4
<4 5
Link5 9
>9 :
(: ;
); <
;< =
} à
aD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\LinkDto.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
DTOs% )
{ 
public 

class 
LinkDto 
{ 
public 
string 
Href 
{ 
get  
;  !
set" %
;% &
}' (
=) *
default+ 2
!2 3
;3 4
public 
string 
Rel 
{ 
get 
;  
set! $
;$ %
}& '
=( )
default* 1
!1 2
;2 3
public 
string 
Method 
{ 
get "
;" #
set$ '
;' (
}) *
=+ ,
default- 4
!4 5
;5 6
}		 
}

 Ñ

jD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\CreateProductDto.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
DTOs% )
;) *
public 
class 
CreateProductDto 
{ 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
=% &
default' .
!. /
;/ 0
public 

string 
? 
Description 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 

string 
? 
ImageUrl 
{ 
get !
;! "
set# &
;& '
}( )
public 

decimal 
Price 
{ 
get 
; 
set  #
;# $
}% &
public		 

int		 
Amount		 
{		 
get		 
;		 
set		  
;		  !
}		" #
public

 

int

 

CategoryId

 
{

 
get

 
;

  
set

! $
;

$ %
}

& '
} ‰
kD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\CreateCategoryDto.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
DTOs% )
;) *
public 
class 
CreateCategoryDto 
{ 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
=% &
default' .
!. /
;/ 0
public 

string 
? 
ImageUrl 
{ 
get !
;! "
set# &
;& '
}( )
public 

int 
? 
ParentCategoryId  
{! "
get# &
;& '
set( +
;+ ,
}- .
} æ

eD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\CategoryDto.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
DTOs% )
;) *
public 
class 
CategoryDto 
{ 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
=% &
default' .
!. /
;/ 0
public		 

string		 
?		 
ImageUrl		 
{		 
get		 !
;		! "
set		# &
;		& '
}		( )
public

 

int

 
?

 
ParentCategoryId

  
{

! "
get

# &
;

& '
set

( +
;

+ ,
}

- .
public 

List 
< 
Link 
> 
Links 
{ 
get !
;! "
set# &
;& '
}( )
=* +
new, /
List0 4
<4 5
Link5 9
>9 :
(: ;
); <
;< =
}  
sD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\DTOs\CatalogItemUpdatedMessage.cs
public 
class %
CatalogItemUpdatedMessage &
{ 
public 

int 
Id 
{ 
get 
; 
set 
; 
} 
public 

string 
Name 
{ 
get 
; 
set !
;! "
}# $
public 

decimal 
Price 
{ 
get 
; 
set  #
;# $
}% &
} Ó	
`D:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\Common\Link.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
Common% +
{ 
public		 

class		 
Link		 
{

 
public 
string 
Href 
{ 
get  
;  !
set" %
;% &
}' (
public 
string 
Rel 
{ 
get 
;  
set! $
;$ %
}& '
public 
string 
Method 
{ 
get "
;" #
set$ '
;' (
}) *
public 
Link 
( 
string 
href 
,  
string! '
rel( +
,+ ,
string- 3
method4 :
): ;
{ 	
Href 
= 
href 
; 
Rel 
= 
rel 
; 
Method 
= 
method 
; 
} 	
} 
} ¢
eD:\EBSCO\GitHub\DotNET_Mentoring_Program_Advanced_2025\CatalogService.Application\AWS\SqsPublisher.cs
	namespace 	
CatalogService
 
. 
Application $
.$ %
AWS% (
{ 
public 

class 
SqsPublisher 
: 
ISqsPublisher  -
{		 
private

 
readonly

 

IAmazonSQS

 #
_sqs

$ (
;

( )
private 
readonly 
string 
	_queueUrl  )
;) *
public 
SqsPublisher 
( 

IAmazonSQS &
sqs' *
,* +
IConfiguration, :
config; A
)A B
{ 	
_sqs 
= 
sqs 
; 
	_queueUrl 
= 
config 
[ 
$str ?
]? @
;@ A
} 	
public 
async 
Task *
PublishCatalogItemUpdatedAsync 8
(8 9%
CatalogItemUpdatedMessage9 R
messageS Z
)Z [
{ 	
var 
body 
= 
JsonSerializer %
.% &
	Serialize& /
(/ 0
message0 7
)7 8
;8 9
await 
_sqs 
. 
SendMessageAsync '
(' (
	_queueUrl( 1
,1 2
body3 7
)7 8
;8 9
} 	
} 
} 