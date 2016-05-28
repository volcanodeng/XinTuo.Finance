

--��������
CREATE TABLE [Finance_RegionRecord](
	[RegionId] [int] not null,					--���ش��룬6λ���֣�ÿ2λ��ʾһ����������
	[CityId] [int] NULL,						--���д���
	[ProvinceId] [int] NULL,					--ʡ�ݴ���
	[CountyName] [nvarchar](255) NULL,			--��������
	[CityName] [nvarchar](255) NULL,			--��������
	[ProvinceName] [nvarchar](255) NULL,		--ʡ������
	[RegionNamePinyin] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[RegionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go


--��˾��Ϣ
CREATE TABLE [Finance_CompanyRecord](
	[CompanyId] uniqueidentifier not null,		  --��˾���루guid��
	[ComFullName] [nvarchar](255) NULL,			  --��˾ȫ��
	[ComShortName] [nvarchar](255) NULL,		  --��˾���
	[RegionId] [int] NULL,						  --�������������� Finance_RegionRecord.RegionId
	[ComAddress] [nvarchar](255) NULL,			  --��˾��ַ
	[ComTel] [nvarchar](255) NULL,				  --��˾�绰
	[ContactsName] [nvarchar](255) NULL,		  --��ϵ������
	[ContactsMobile] [nvarchar](255) NULL,		  --��ϵ���ֻ�
	[ContactsEmail] [nvarchar](255) NULL,		  --��ϵ���ʼ�
	[ContactsUserAccount] [nvarchar](255) NULL,   --��ϵ��ϵͳ�˻������� Orchard_Users_UserPartRecord.UserName
PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go

--��Ŀ���
CREATE TABLE [Finance_SubjectCategoryRecord](
	[SubjectCategory] [int] IDENTITY(1,1) NOT NULL,	--�����
	[ParentSubjectCategory] [int] NULL,				--�������
	[CategoryFullName] [nvarchar](255) NULL,		--���
	[CategoryShortName] [nvarchar](255) NULL,		--�����
PRIMARY KEY CLUSTERED 
(
	[SubjectCategory] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


--��Ŀ
CREATE TABLE [Finance_SubjectsRecord](
	[SubjectCode] [int] NOT NULL,				--��Ŀ��ţ�¼�룩
	[ParentSubjectCode] [int] NULL,				--������Ŀ���
	[Level] [int] NULL,							--��Ŀ����
	[SubjectName] [nvarchar](255) NULL,			--��Ŀ����
	[SubjectCategory] [int] NULL,				--��Ŀ���,����Finance_SubjectCategoryRecord.SubjectCategory
	[CompanyId] [int] NULL,						--��Ŀ������˾,����Finance_CompanyRecord.CompanyId
	[BalanceDirection] [nvarchar](255) NULL,	--�������
	[BeginBalance] [decimal](19, 5) NULL,		--�ڳ����
	[EndBalance] [decimal](19, 5) NULL,			--��ĩ���
	[SubjectState] [int] NULL,					--��Ŀ״̬��1 ������0 ͣ��
	[NamePath] [nvarchar](255) NULL,			--ȫ�����Ŀ���ƣ���-ƴ�ӣ�
	[CodePath] [nvarchar](255) NULL,			--ȫ�����Ŀ���루��-ƴ�ӣ�
	[Debit] [decimal](19, 5) NULL,				--֧��
	[Credit] [decimal](19, 5) NULL,				--����
	[SubjectAmount] [decimal](19, 5) NULL,		--��Ŀ���
	[Creator] [nvarchar](255) NULL,				--��Ŀ������
	[CreateTime] [datetime] NULL,				--��Ŀ����ʱ��
	[LastUpdate] [nvarchar](255) NULL,			--��������
	[LastUpdateTime] [datetime] NULL,			--������ʱ��
PRIMARY KEY CLUSTERED 
(
	[SubjectCode] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

