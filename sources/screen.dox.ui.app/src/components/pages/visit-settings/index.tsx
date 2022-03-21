
import React, { ChangeEvent } from 'react';
import { connect } from 'react-redux';
import { Dispatch } from 'redux';
import { IRootState } from '../../../states';
import { TextField, Button } from '@material-ui/core';
import { ContentContainer, ResetTitleText, ResetText, ResetDescriptionText } from '../styledComponents';
import { getVisitSettingsRequest, VisitSettingsResponseItem, getVisitSettingsRequestSuccess, updateVisitSettingsRequest  } from 'actions/visit-settings';
import { getVisitSettingsListSelector, isVisitSettingsListLoadingSelector } from 'selectors/visit-settings'
import { TableBody, TableCell, TableContainer, TableHead, TableRow, Table, Paper, Grid, TableSortLabel, Box, CircularProgress, Input, Switch, } from '@material-ui/core';
import customClasss from  '../pages.module.scss';
import { ClassCompomentstyles } from '../styledComponents';
import { withStyles } from '@material-ui/core/styles';
import { setCurrentPage } from '../../../actions/settings';
import { ERouterKeys, ERouterUrls } from '../../../router';


export interface VisitSettingsProps {
    getVisitSettings?: () => void;
    visitSettings: Array<VisitSettingsResponseItem>;
    updateVisitSettings?: () => void;
    isLoading: boolean;
    classes: any;
    setVisitSettings: (res: Array<VisitSettingsResponseItem>) => void;
    setCurrentPage?: (k: string, p: string) => void;
}

export interface VisitSettingsState {
    visitSettings: Array<VisitSettingsResponseItem>;
}

class VisitSettings extends React.Component<VisitSettingsProps, VisitSettingsState> {

    constructor(props: VisitSettingsProps) {
        super(props);
        this.state = { visitSettings: [] };
        this.handleChange = this.handleChange.bind(this);
        this.handleCheck = this.handleCheck.bind(this);
        this.updateVisitSettings  = this.updateVisitSettings.bind(this);
    }

    public componentDidMount() {
        this.props.getVisitSettings && this.props.getVisitSettings();
        this.props.setCurrentPage && this.props.setCurrentPage(ERouterKeys.VISIT_SETTINGS, ERouterUrls.VISIT_SETTINGS);
    }

    public componentWillUnmount() {
        this.setState({
            visitSettings: this.props.visitSettings
        })
    }

    public componentWillMount() {
        this.setState({
            visitSettings: this.props.visitSettings
        })
    }

    public componentWillUpdate() {
        console.log('update');
    }

    public componentWillReceiveProps(props: VisitSettingsProps) {
        this.setState({
            visitSettings: props.visitSettings
        })
    }

    public handleChange(e: any, index: number) {
        var cutScore = parseInt(e.target.value);
        if(isNaN(cutScore)) {
            cutScore = 0;
        }
        var visitSettings = this.props.visitSettings;
        visitSettings[index] = {
            ...visitSettings[index],
            CutScore: cutScore,
        }
        this.setState({
            visitSettings: visitSettings
        })

        this.props.setVisitSettings && this.props.setVisitSettings(visitSettings);
    }

    public handleCheck(e:any, index: number) {
        var checked = e.target.checked;
        var visitSettings = this.props.visitSettings;
        visitSettings[index] = {
            ...visitSettings[index],
            IsEnabled: checked,
        }
        this.setState({
            visitSettings: visitSettings
        })

        this.props.setVisitSettings && this.props.setVisitSettings(visitSettings);
    }

    public updateVisitSettings () {
        this.props.updateVisitSettings && this.props.updateVisitSettings();
    }

     render(): React.ReactElement {
        
        return (
          <ContentContainer>
           <Grid container spacing={1} style={{ fontSize: 14 }}>
               <Grid container style={{ paddingBottom: 30 }}>
                    <Grid item sm={8} xs={12}>
                        <ResetTitleText>
                            Visit Settings
                        </ResetTitleText>
                        <ResetDescriptionText>
                            Turn ON each screening tool to display on the visit list patients who have a positive screen for that item. Enter the cut score to display on the visit list patients who have a severity level that is equal or greater to the cut score.
                        </ResetDescriptionText>
                    </Grid>
                    <Grid item sm={4} xs={12}>
                        <Grid item sm={12} xs={12} style={{ textAlign: 'right', marginTop: 14 }}>
                            <Button 
                                size="large"  
                                disabled={false}
                                variant="contained" 
                                color="primary" 
                                className={customClasss.resetButtonStyle}
                                onClick={this.updateVisitSettings}
                            >
                                <p className={customClasss.resetButtonTextStyle}>Save Changes</p>
                            </Button>
                        </Grid>
                        
                        <Grid item sm={12} xs={12} style={{ textAlign: 'right', marginTop: 14 }}>
                            <Button 
                                size="large"
                                disabled={false}
                                variant="contained" 
                                color="primary" 
                                className={customClasss.resetButtonStyle}
                                onClick={() => { this.props.getVisitSettings && this.props.getVisitSettings() }}
                            >
                                <p className={customClasss.resetButtonTextStyle}>Reset</p>
                            </Button>
                        </Grid>
                        
                    </Grid>
               </Grid>
                <Grid item xs={12}>
                {this.props.isLoading?<CircularProgress disableShrink={false} className={customClasss.circularLoadingStyle}/>: 
                    <TableContainer>
                        <Table style={{ borderCollapse: 'inherit' }}>
                            <TableHead className={customClasss.tableHead}>
                                <TableRow>
                                    <TableCell>Screening Tool</TableCell>
                                    <TableCell>Cut Score</TableCell>
                                    <TableCell className={customClasss.rightTh}>Turn OFF/ON</TableCell>
                                </TableRow>
                            </TableHead>
                            
                            <TableBody>
                                {this.state.visitSettings && this.state.visitSettings.map((visitSettingsItem: VisitSettingsResponseItem, index: number) => (
                                    <TableRow key={index}>
                                        <TableCell style={{ fontSize: '1em' }}>{visitSettingsItem.Name}</TableCell>
                                        <TableCell>
                                            {visitSettingsItem.SectionScoreHint &&
                                            <Grid container spacing={1}>
                                                <Grid item sm={2}>
                                                    <TextField
                                                        margin="dense"
                                                        variant="outlined"
                                                        value={visitSettingsItem.CutScore}
                                                        className={customClasss.visitSettingCutScore}
                                                        onChange={(e) => {
                                                            this.handleChange(e, index);
                                                        }}
                                                    />
                                                </Grid>
                                                <Grid item sm={10} className={customClasss.verticalMiddleStyle}>
                                                    <p style={{ fontSize: '14px' }}>{visitSettingsItem.SectionScoreHint}</p>
                                                </Grid>
                                            </Grid>}
                                        </TableCell>
                                        <TableCell className={customClasss.rightTh}>
                                            <Switch  
                                                checked={visitSettingsItem.IsEnabled} 
                                                onChange={(e) => this.handleCheck(e, index)}
                                                classes={{
                                                    root: this.props.classes.element,
                                                    switchBase: this.props.classes.switchBase,
                                                    thumb: this.props.classes.thumb,
                                                    track: this.props.classes.track,
                                                    checked: this.props.classes.checked
                                                }}
                                                focusVisibleClassName={this.props.classes.focusVisible}
                                            />
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer> }   
                </Grid>
           </Grid>
          </ContentContainer>
        )
    }
}


const mapStateToPtops = (state: IRootState) => ({
    visitSettings: getVisitSettingsListSelector(state),
    isLoading: isVisitSettingsListLoadingSelector(state),
})

const mapDispatchToProps = (dispatch: Dispatch) => {
    return {
        getVisitSettings: () => dispatch(getVisitSettingsRequest()),
        setVisitSettings: (value: Array<VisitSettingsResponseItem>) => dispatch(getVisitSettingsRequestSuccess(value)),
        updateVisitSettings: () => dispatch(updateVisitSettingsRequest()),
        setCurrentPage: (key: string, path: string) => {
            dispatch(setCurrentPage(key, path));
        }  
    }
}

export default connect(mapStateToPtops, mapDispatchToProps)(withStyles(ClassCompomentstyles, { withTheme: true })(VisitSettings));