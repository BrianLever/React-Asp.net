import React from 'react';
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { ERouterKeys, ERouterUrls } from '../../../router';
import { setCurrentPage } from '../../../actions/settings';
import { IRootState } from '../../../states';
import LicenseKeysList from './license-keys-list';


export interface ILicenseKeysProps {
    setCurrentPage?: (k: string, p: string) => void;
}
export interface ILicenseKeysState {}

class LicenseKeys extends React.Component<ILicenseKeysProps, ILicenseKeysState> {

    public componentDidMount() {
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.LICENSE_KEYS, ERouterUrls.LICENSE_KEYS);
    }

    public render():React.ReactNode {
        return <LicenseKeysList />
    }

}

const mapStateToProps = (state: IRootState) => ({})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }   
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(LicenseKeys);