

--行政区划
CREATE TABLE [Finance_RegionRecord](
	[RegionId] [int] not null,					--区县代码，6位数字，每2位表示一个区划级别
	[CityId] [int] NULL,						--城市代码
	[ProvinceId] [int] NULL,					--省份代码
	[CountyName] [nvarchar](255) NULL,			--区县名称
	[CityName] [nvarchar](255) NULL,			--城市名称
	[ProvinceName] [nvarchar](255) NULL,		--省份名称
	[RegionNamePinyin] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go


--公司信息
CREATE TABLE [Finance_CompanyRecord](
	[CompanyId] uniqueidentifier not null,		  --公司编码（guid）
	[ComFullName] [nvarchar](255) NULL,			  --公司全称
	[ComShortName] [nvarchar](255) NULL,		  --公司简称
	[RegionId] [int] NULL,						  --行政区划，关联 Finance_RegionRecord.RegionId
	[ComAddress] [nvarchar](255) NULL,			  --公司地址
	[ComTel] [nvarchar](255) NULL,				  --公司电话
	[ContactsName] [nvarchar](255) NULL,		  --联系人姓名
	[ContactsMobile] [nvarchar](255) NULL,		  --联系人手机
	[ContactsEmail] [nvarchar](255) NULL,		  --联系人邮件
	[ContactsUserAccount] [nvarchar](255) NULL,   --联系人系统账户，关联 Orchard_Users_UserPartRecord.UserName
PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go

--科目类别
CREATE TABLE [Finance_SubjectCategoryRecord](
	[SubjectCategory] [int] IDENTITY(1,1) NOT NULL,	--类别编号
	[ParentSubjectCategory] [int] NULL,				--父级编号
	[CategoryFullName] [nvarchar](255) NULL,		--类别
	[CategoryShortName] [nvarchar](255) NULL,		--类别简称
PRIMARY KEY CLUSTERED 
(
	[SubjectCategory] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


--科目
CREATE TABLE [Finance_SubjectsRecord](
	[SubjectCode] [int] NOT NULL,				--科目编号（录入）
	[ParentSubjectCode] [int] NULL,				--父级科目编号
	[Level] [int] NULL,							--科目级别
	[SubjectName] [nvarchar](255) NULL,			--科目名称
	[SubjectCategory] [int] NULL,				--科目类别,关联Finance_SubjectCategoryRecord.SubjectCategory
	[CompanyId] [int] NULL,						--科目所属公司,关联Finance_CompanyRecord.CompanyId
	[BalanceDirection] [nvarchar](255) NULL,	--借贷方向
	[BeginBalance] [decimal](19, 5) NULL,		--期初余额
	[EndBalance] [decimal](19, 5) NULL,			--期末余额
	[SubjectState] [int] NULL,					--科目状态：1 开启，0 停用
	[NamePath] [nvarchar](255) NULL,			--全级别科目名称（用-拼接）
	[CodePath] [nvarchar](255) NULL,			--全级别科目代码（用-拼接）
	[Debit] [decimal](19, 5) NULL,				--支出
	[Credit] [decimal](19, 5) NULL,				--收入
	[SubjectAmount] [decimal](19, 5) NULL,		--科目金额
	[Creator] [nvarchar](255) NULL,				--科目创建者
	[CreateTime] [datetime] NULL,				--科目创建时间
	[LastUpdate] [nvarchar](255) NULL,			--最后更新人
	[LastUpdateTime] [datetime] NULL,			--最后更新时间
PRIMARY KEY CLUSTERED 
(
	[SubjectCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

