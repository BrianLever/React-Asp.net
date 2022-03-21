import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import AddIcon from '@material-ui/icons/Add';
import { Button, Grid } from '@material-ui/core';
import { 
    getManageDevicesTotalSelector, isManageListLoadingSelector, getManageDevicesCurrentPageSelector,
    getManageDevicesListSelector, getCurrentActiveDirectionSelector, getCurrentActiveKeySelector
} from '../../../../selectors/manage-devices';
import ScreendoxTable, { EScreendoxTableType } from '../../../UI/table';
import { 
    changeActiveSortObjectAction, changeCurrentPageRequest, getEditKioskDetailsByIdRequest, IManegeDevicesListResponse, manageDeviceAutoRequest,
} from '../../../../actions/manage-devices';
import { ButtonTextStyle, ContentContainer, TitleText } from '../../styledComponents';
import { convertDateToStringFormat } from '../../../../helpers/dateHelper';
import classes from  '../../pages.module.scss';
import ScreendoxModal from '../../../UI/modal';
import { closeModalWindow, EModalWindowKeys, openModalWindow } from '../../../../actions/settings';
import AddNewDeviceContent from '../add-new-device-content';
import AddNewDeviceButtons from '../add-new-device-buttons';
import EditKioskDetailsContent from '../edit-kiosk-details-content';
import EditKioskDetailsActions from '../edit-kiosk-details-actions';
import ScreendoxEditButton from '../../../UI/custom-buttons/editButton';
import { getListBranchLocationsSelector, getScreeningProfileListSelector } from '../../../../selectors/shared';
import { getListBranchLocationsRequest,getScreeningProfileListRequest,TBranchLocationsItemResponse, TScreeningProfileItemResponse } from '../../../../actions/shared';
import { useHistory } from 'react-router-dom';
import CustomAlert from 'components/UI/alert';


const ManageDevicesList = (): React.ReactElement => {

  const dispatch = useDispatch();
  const history = useHistory();
  const screenList = useSelector(getManageDevicesListSelector);
  const isLoading = useSelector(isManageListLoadingSelector);
  const sortKey = useSelector(getCurrentActiveKeySelector);
  const totalItems = useSelector(getManageDevicesTotalSelector);
  const currentPage = useSelector(getManageDevicesCurrentPageSelector);
  const sortDirection = useSelector(getCurrentActiveDirectionSelector);
  const locationsList:  Array<TBranchLocationsItemResponse> = useSelector(getListBranchLocationsSelector);
 
  React.useEffect(() => {
    if(history.location.search !== '') {
      const deviceId = parseInt(history.location.search.replace('?', ''));
      dispatch(getEditKioskDetailsByIdRequest(deviceId));
    }
    dispatch(manageDeviceAutoRequest())
  }, [])
  return (
      <ContentContainer>
        <CustomAlert />
        <Grid container justifyContent="flex-start" alignContent="center" alignItems="center" spacing={2} style={{ marginBottom: '10px' }}>
          <Grid item xs={6} style={{ textAlign: 'start' }}>
            <TitleText>
                Manage Devices
            </TitleText>
          </Grid>
          <Grid item xs={6} style={{ textAlign: 'end' }}>
            <Button startIcon={<AddIcon />} 
              size="large"  
              disabled={false}
              className={classes.removeBoxShadow}
              variant="contained" 
              color="primary" 
              style={{ backgroundColor: '#2e2e42' }}
              onClick={() => {
                dispatch(openModalWindow(EModalWindowKeys.manageDevicesAddNewDevice));
              }}
              >
                <ButtonTextStyle>Add New Device</ButtonTextStyle>
              </Button>
          </Grid>
        </Grid>
        <ScreendoxTable
          isLoading={isLoading}
          type={EScreendoxTableType.screenList}
          total={totalItems}
          isCollapsable="NO"
          onPageClick={(page: number) => {
            dispatch(changeCurrentPageRequest(page));
          }}
          currentPage={currentPage}
          isAutoRefreshDisabled={true}
          isPagination={true}
          selectedItemValue={null}
          onSelectHandler={(v: number) => {
          }}
          onTriggerAutoUpdate={() => {

          }}
          headers={
            [
              { 
                name: 'Key',
                key: 'KioskID',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Device Name',
                key: 'KioskName',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name: 'Branch Location',
                key: 'BranchLocationName',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Screen Profile',
                key: 'ScreeningProfileName',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Last Activity Time',
                key: 'LastActivityDate',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'IP Address',
                key: 'IpAddress',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Version',
                key: 'KioskAppVersion',
                action: changeActiveSortObjectAction,
                active: sortKey,
                direction: sortDirection,
              },
              { 
                name:  'Action',
                key: 'Action',
              },
            ]
          }
          rows={screenList.map((r: IManegeDevicesListResponse) => {
            var locationData = locationsList.filter(item => item.ID == r.BranchLocationID);
            return { 
                KioskKey: r.KioskKey,
                Name: r.Name,
                BranchLocationID: locationData && locationData[0]?.Name,
                ScreeningProfileName: r.ScreeningProfileName,
                LastActivityDate: r.LastActivityDate ? convertDateToStringFormat(r.LastActivityDate) : '',
                IpAddress: r.IpAddress,
                KioskAppVersion: r.KioskAppVersion,
                Action: (
                  <ScreendoxEditButton 
                    onClickHandler={e => {
                      e.stopPropagation();
                      if (r.KioskID) {
                        history.push('?'+r.KioskID)
                        dispatch(getEditKioskDetailsByIdRequest(r.KioskID));
                      }
                    }}
                  />
                ),
              onSelectItem: () => { 
                if (r.KioskID) {
                  // dispatch(getEditKioskDetailsByIdRequest(r.KioskID));
                }
              },
              subItem: [],
              // disabled: r.Disabled
              }
          })}
        />
      <ScreendoxModal
        uniqueKey={EModalWindowKeys.manageDevicesAddNewDevice}
        content={<AddNewDeviceContent />}
        actions={<AddNewDeviceButtons />}
        title="Add New Device"
        onConfirm={() => {
          dispatch(closeModalWindow(EModalWindowKeys.manageDevicesAddNewDevice));
        }}
      />
      <ScreendoxModal
        uniqueKey={EModalWindowKeys.manageDevicesEditKioskDetails}
        content={<EditKioskDetailsContent />}
        actions={<EditKioskDetailsActions />}
        title="Edit Device"
        onConfirm={() => {
          dispatch(closeModalWindow(EModalWindowKeys.manageDevicesEditKioskDetails));
        }}
      />
      </ContentContainer>
  )
}

export default ManageDevicesList;
