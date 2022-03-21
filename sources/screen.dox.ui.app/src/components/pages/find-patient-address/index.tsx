import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import FindPatientAddressDetail from './find-patient-address-detail';
import { getScreeningReportRequest } from 'actions/screen/report';
import { getFindPatientAddressEhrRecordPatientRequest } from 'actions/find-patient-address';

export interface IFindPatientAddressProps {
    setCurrentPage?: (k: string, p: string) => void;
    getScreeningReportRequest?: (reportId: number) => void;
    getFindPatientAddressEhrRecordPatientRequest?: (reportId: number) => void;
}
export interface IFindPatientAddressState {}

class FindPatientAddress extends React.Component<IFindPatientAddressProps, IFindPatientAddressState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.FIND_PATIENT_ADDRESS, ERouterUrls.FIND_PATIENT_ADDRESS);
    }

    public render():React.ReactNode {
        return <FindPatientAddressDetail />
    }

}

const mapStateToPtops = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        },
        getScreeningReportRequest: (reportId: number) => {
            dispatch(getScreeningReportRequest(reportId))
        },
        getFindPatientAddressEhrRecordPatientRequest: (reportId: number) => {
            dispatch(getFindPatientAddressEhrRecordPatientRequest(reportId))
        }
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(FindPatientAddress);