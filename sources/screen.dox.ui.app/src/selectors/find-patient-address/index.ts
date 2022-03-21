import { IRootState } from '../../states';

export const findPatientAddressCurrentPageSelector = (state: IRootState) => state.findPatientAddress.currentPage;
export const findPatientAddressTotalSelector = (state: IRootState) => state.findPatientAddress.ehrRecordsTotal;
export const isFindPatientAddressEhrRecordPatientLoadingSelector = (state: IRootState) => state.findPatientAddress.isEhrRecordPatientsLoading;
export const isFindPatientAddressehrRecordPatientLoadingErrorSelector = (state: IRootState) => state.findPatientAddress.isEhrRecordPatientsLoadingError;
export const findPatientAddressEhrRecordPatientsSelector = (state: IRootState) => state.findPatientAddress.ehrRecordsPatients;
export const findPatientAddressPhoneNumberSelector = (state: IRootState) => state.findPatientAddress.phoneNumber;
export const findPatientAddressStreetAddressSelector = (state: IRootState) => state.findPatientAddress.streetAddress;
export const findPatientAddressStateSelector = (state: IRootState) => state.findPatientAddress.state;
export const findPatientAddressPatientIdSelector = (state: IRootState) => state.findPatientAddress.patientID;
export const findPatientAddressCitySelector = (state: IRootState) => state.findPatientAddress.city;
export const findPatientAddressZipcodeSelector = (state: IRootState) => state.findPatientAddress.zipCode;
export const getFindPatientAddressEhrExportPatientRecordSelectedIdSelector = (state: IRootState) => state.findPatientAddress.ehrExportPatientRecordSelectedId;
