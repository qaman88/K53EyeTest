namespace ExpertWaves {
	namespace Data {
		public abstract class IBaseData {
			public virtual void onLoadFileNotFound() { }
			public virtual void onBeforeLoad() { }
			public virtual void onAfterLoad() { }
			public virtual void onBeforeSave() { }
			public virtual void onAfterSave() { }
		}

	}
}