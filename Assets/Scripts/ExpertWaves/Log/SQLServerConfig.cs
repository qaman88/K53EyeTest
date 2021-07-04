using log4net.Appender;
using log4net.Layout;

namespace ExpertWaves {
	namespace Log {
		public class SQLServerConfig {
			private string publicKeyToken = "b77a5c561934e089";
			private string version = "1.0.3300.0";
			private string culture = "neutral";
			private string source = "SQLSVR";
			private string catalog = "test_log4net";
			private bool integratedSecurity = false;
			private bool persistSecurity = true;
			private string user = "sa";
			private string passowrd = "sa";
			private string name = "AdoNetAppender";
			private int bufferSize = 1;
			private AdoNetAppender adoNetAppenderSQL;
			public string PublicKeyToken { get => this.publicKeyToken; set => this.publicKeyToken = value; }
			public string Version { get => this.version; set => this.version = value; }
			public string Culture { get => this.culture; set => this.culture = value; }
			public string Source { get => this.source; set => this.source = value; }
			public string Catalog { get => this.catalog; set => this.catalog = value; }
			public bool IntegratedSecurity { get => this.integratedSecurity; set => this.integratedSecurity = value; }
			public bool PersistSecurity { get => this.persistSecurity; set => this.persistSecurity = value; }
			public string User { get => this.user; set => this.user = value; }
			public string Passowrd { get => this.passowrd; set => this.passowrd = value; }
			public string Name { get => this.name; set => this.name = value; }
			public int BufferSize { get => this.bufferSize; set => this.bufferSize = value; }

			public SQLServerConfig() {
				adoNetAppenderSQL = new AdoNetAppender();
			}

			// Command Text
			public string CommandText { get => this.getCommandText(); }
			private string getCommandText() {
				return "INSERT INTO Log " +
						"([Date],[Level],[Logger],[Message],[Exception]) VALUES " +
						"(@log_date, @log_level, @log_logger, @log_message,@log_exception])";
			}

			// Connection String
			public string ConnectionString { get => this.getConnectionString(); }
			private string getConnectionString() {
				return $"data source={Source};" +
						 $"initial catalog={Catalog};" +
						 $"integrated security={IntegratedSecurity};" +
						 $"persist security info={PersistSecurity};" +
						 $"User ID={User};" +
						 $"Password={Passowrd}";
			}

			// Connection Type
			public string ConnectionType { get => this.getConnectionType(); }
			private string getConnectionType() {
				return $"System.Data..SqlConnection, " +
					 $"System.Data, " +
					 $"Version={Version}, " +
					 $"Culture={Culture}, " +
					 $"PublicKeyToken={PublicKeyToken}";
			}


			// UCT datetime column
			public AdoNetAppenderParameter Date { get => this.getDate(); }
			private AdoNetAppenderParameter getDate() {
				return new AdoNetAppenderParameter {
					ParameterName = "@log_date",
					DbType = System.Data.DbType.DateTime,
					Layout = new RawUtcTimeStampLayout()
				};
			}

			// Level column
			public AdoNetAppenderParameter Level { get => this.getLevel(); }
			private AdoNetAppenderParameter getLevel() {
				return new AdoNetAppenderParameter {
					ParameterName = "@log_level",
					DbType = System.Data.DbType.String,
					Size = 50,
					Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level"))
				};
			}

			// Thread column
			public AdoNetAppenderParameter Thread { get => this.getThread(); }
			private AdoNetAppenderParameter getThread() {
				return new AdoNetAppenderParameter {
					ParameterName = "@thread",
					DbType = System.Data.DbType.String,
					Size = 255,
					Layout = new Layout2RawLayoutAdapter(new PatternLayout("Thread%thread"))
				};
			}

			// Logger column
			public AdoNetAppenderParameter Logger { get => this.getLogger(); }
			private AdoNetAppenderParameter getLogger() {
				return new AdoNetAppenderParameter {
					ParameterName = "@logger",
					DbType = System.Data.DbType.String,
					Size = 255,
					Layout = new Layout2RawLayoutAdapter(new PatternLayout("%logger"))
				};
			}

			// Message Column
			public AdoNetAppenderParameter Message { get => this.getMessage(); }
			private AdoNetAppenderParameter getMessage() {
				return new AdoNetAppenderParameter {
					ParameterName = "@message",
					DbType = System.Data.DbType.String,
					Size = 4000,
					Layout = new Layout2RawLayoutAdapter(new PatternLayout("%message"))
				};
			}

			// Exception Column
			public AdoNetAppenderParameter Exception { get => this.getException(); }
			private AdoNetAppenderParameter getException() {
				return new AdoNetAppenderParameter {
					ParameterName = "@exception",
					DbType = System.Data.DbType.String,
					Size = 4000,
					Layout = new Layout2RawLayoutAdapter(new ExceptionLayout())
				};
			}

			// AdoNetAppender
			public void ConfigureAdoNetAppender() {
				adoNetAppenderSQL.Name = Name;
				adoNetAppenderSQL.BufferSize = BufferSize;
				adoNetAppenderSQL.CommandText = CommandText;
				adoNetAppenderSQL.ConnectionString = ConnectionString;
				adoNetAppenderSQL.ConnectionType = ConnectionType;
				adoNetAppenderSQL.AddParameter(Date);
				adoNetAppenderSQL.AddParameter(Level);
				adoNetAppenderSQL.AddParameter(Thread);
				adoNetAppenderSQL.AddParameter(Logger);
				adoNetAppenderSQL.AddParameter(Message);
				adoNetAppenderSQL.AddParameter(Exception);
				adoNetAppenderSQL.ActivateOptions();
			}

			public AdoNetAppender AdoNetAppender { get => this.getAdoNetAppender(); }
			public AdoNetAppender AdoNetAppenderSQL { get => this.adoNetAppenderSQL; set => this.adoNetAppenderSQL =  value ; }

			public AdoNetAppender getAdoNetAppender() {
				return adoNetAppenderSQL;
			}
		}
	}
}
