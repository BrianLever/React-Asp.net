import { IRootState } from '../../states';
import { 
    IKioskDetailsResponse, IManegeDevicesListResponse,  
} from '../../actions/manage-devices';

export const getManageDevicesListSelector = (s: IRootState): Array<IManegeDevicesListResponse> =>  s.manageDevice.manageDevicesList;
export const getManageDevicesTotalSelector = (s: IRootState): number =>  s.manageDevice.totalCount;
export const getManageDevicesCurrentPageSelector = (s: IRootState): number =>  s.manageDevice.currentPage;
export const isManageListLoadingSelector = (s: IRootState): boolean =>  s.manageDevice.isManagedDevicesListLoading;
export const getCurrentActiveDirectionSelector = (s: IRootState): string =>  s.manageDevice.activeDirection;
export const getCurrentActiveKeySelector = (s: IRootState): string =>  s.manageDevice.activeSort;
export const getSelectedAddNewKioskBranchLocationIdSelector = (s: IRootState): number =>  s.manageDevice.selectedAddNewKioskBranchLocationId;
export const getSelectedAddNewKioskDeviceNameSelector = (s: IRootState): string =>  s.manageDevice.selectedAddNewKioskDeviceName;
export const getSelectedAddNewKioskScreenProfileSelector = (s: IRootState): string =>  s.manageDevice.selectedAddNewKioskScreenProfile;
export const getSelectedAddNewKioskSecretKeySelector = (s: IRootState): string =>  s.manageDevice.selectedAddNewKioskSecretKey;
export const getSelectedAddNewKioskDescriptionSelector = (s: IRootState): string =>  s.manageDevice.selectedAddNewKioskDescription;
export const getInconsistencyInFieldsFlagSelector = (s: IRootState): boolean =>  s.manageDevice.inconsistencyInFieldsFlag;
export const getSelectedFilterBranchLocationIdSelector = (s: IRootState): number =>  s.manageDevice.selectedFilterBranchLocationId;
export const getSelectedFilterBranchLocationNameKeySelector = (s: IRootState): string =>  s.manageDevice.selectedFilterBranchLocationNameKey;
export const getSelectedFilterShowDisabledSelector = (s: IRootState): number =>  s.manageDevice.selectedFilterShowDisabled;
export const getSelectedFilterScreeningProfileIdSelector = (s: IRootState): number =>  s.manageDevice.selectedFilterScreeningProfileId;
export const getEditKioskDetailsSelector = (s: IRootState): IKioskDetailsResponse =>  s.manageDevice.kioskDetails;
export const getEditKioskDetailsBranchLocationSelector = (s: IRootState): number =>  s.manageDevice.kioskEditDetailsBranchLocation;
export const getEditKioskDetailsDescriptionSelector = (s: IRootState): string =>  s.manageDevice.kioskEditDetailsDescription;
export const getEditKioskDetailsCurrentEditKioskIdSelector = (s: IRootState): number =>  s.manageDevice.currentEditKioskId;