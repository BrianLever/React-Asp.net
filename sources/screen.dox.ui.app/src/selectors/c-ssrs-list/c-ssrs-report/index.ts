import { IRootState } from '../../../states';

export const isCssrsReportPatientRecordLoadingSelector = (state: IRootState) => state.cssrsReport.isPatientRecordsLoading;
export const cssrsReportScreendoxPatientRecordsSelector = (state: IRootState) => state.cssrsReport.screendoxPatientRecords;
export const cssrsReportEhrPatientRecordsSelector = (state: IRootState) => state.cssrsReport.ehrPatientRecords;
export const cssrsReportFirstNameSelector = (state: IRootState) => state.cssrsReport.firstName;
export const cssrsReportLastNameSelector = (state: IRootState) => state.cssrsReport.lastName;
export const cssrsReportMiddleNameSelector = (state: IRootState) => state.cssrsReport.middleName;
export const cssrsReportBirthdaySelector = (state: IRootState) => state.cssrsReport.dateofBirth;
export const cssrsReportLocationsSelector = (state: IRootState) => state.cssrsReport.locations;
export const cssrsReportLocationIdSelector = (state: IRootState) => state.cssrsReport.locationId;
export const cssrsReportDetailSelector = (state: IRootState) => state.cssrsReport.cssrsReport;
export const isCssrsReportDetailLoadingSelector = (state: IRootState) => state.cssrsReport.isLoading;
export const cssrsReportEhrExportPatientRecordSelectedIdSelector = (state: IRootState) => state.cssrsReport.ehrExportPatientRecordSelectedId;
export const isReportCreateLoadingSelector = (state: IRootState) => state.cssrsReport.isReportCreateLoading;