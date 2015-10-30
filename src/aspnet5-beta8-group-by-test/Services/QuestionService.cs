using aspnet5_beta8_group_by_test.Data;
using aspnet5_beta8_group_by_test.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet5_beta8_group_by_test.Services
{
    public class QuestionService
    {
		private readonly ApplicationDbContext _dbContext;

		public QuestionService(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		protected ApplicationDbContext DbContext
		{
			get
			{
				return _dbContext;
			}
		}

		public class GroupByResult
		{
			public int Key { get; set; }

			public DateTime MaxDate { get; set; }
		}

		public ExamInstanceQuestion[] BaselineQuery()
		{
			var query = from instance in this.DbContext.ExamInstances
						join question in this.DbContext.ExamInstanceQuestions
							on instance.Id equals question.ExamInstanceId
						where instance.Id == 3
						select question;

			return query.ToArray();
		}

		public GroupByResult[] Query1()
		{
			var query = from instance in this.DbContext.ExamInstances
						join question in this.DbContext.ExamInstanceQuestions
							on instance.Id equals question.ExamInstanceId
						where instance.Id == 3
						group question by question.QuestionId into gQuestions
						select new GroupByResult()
						{
							Key = gQuestions.Key,
							MaxDate = gQuestions.Max(q => q.UtcDateUpdated)
						};

			return query.ToArray();
		}
	}
}
