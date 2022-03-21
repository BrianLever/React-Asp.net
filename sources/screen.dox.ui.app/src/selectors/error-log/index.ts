import { IRootState } from '../../states';


export const getErrorLogsSelector = (state: IRootState) => state.errorLogs.errorLogList;
export const isErrorLogLoadingSelector = (state: IRootState) => state.errorLogs.isErrorLogLoading;
export const getErrorLogCurrentPageSelector = (state: IRootState) => state.errorLogs.currentPage;
export const getErrorLogStartDateSelector = (state: IRootState) => state.errorLogs.startDate;
export const getErrorLogEndDateSelector = (state: IRootState) => state.errorLogs.endDate;
export const getErrorLogTotalCountSelector = (state: IRootState) => state.errorLogs.totalErrorLogs;
export const getErrorLogItemSelector = (state: IRootState) => state.errorLogs.errorLogItem;
export const isErrorLogDetailLoadingSelector = (state: IRootState) => state.errorLogs.isErrorLogDetailLoading;

