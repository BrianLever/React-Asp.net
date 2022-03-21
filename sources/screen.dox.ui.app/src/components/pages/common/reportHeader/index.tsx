import React from 'react';
import momnet from 'moment';
import { Grid } from '@material-ui/core';
import { 
    TopSection, MiddleSection, TopSectionLabel, TopSectionText,
} from '../../styledComponents';
import { Link } from 'react-router-dom';
import { convertDateTimeOffsetToStringFormat, convertDateToStringFormat  } from 'helpers/dateHelper';
export type TReportHeader = {
    srn: number | string;
    showSrnAsLink: boolean;
    zip: string;
    city: string;
    state: string;
    phone: string;
    location: string;
    lastName: string;
    createdBy: string;
    firstName: string;
    birthDate: string;
    middleName: string;
    patientHRN: string;
    createdDate: string;
    mailingAddress: string;
}

const ReportHeader = (props: TReportHeader): React.ReactElement => {
    const { 
        firstName, lastName, middleName, birthDate, city, mailingAddress, patientHRN,
        createdBy, createdDate, location, phone, state, zip, srn, showSrnAsLink = true 
     } = props;
    return (
        <>
            <TopSection>
                <Grid container justifyContent="flex-start" alignContent="center" alignItems="stretch" spacing={2}>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Patient Last Name
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { lastName }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={2} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                First Name
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { firstName }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={2} xs={12} sm={12} >
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Middle Name
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { middleName }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={2} xs={12} sm={12} >
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Date of Birth
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { momnet(birthDate).format("MM/DD/YYYY") }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                HRN
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { patientHRN }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                </Grid>
                <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2}>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Mailing Address
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { mailingAddress }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={2} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                City
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { city }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={2} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                State
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { state }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={2} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Zip Code
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { zip }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Primary Phone Number
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { phone }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                </Grid>
            </TopSection>
             <MiddleSection className="ReportHeader-MiddleSection">
                <Grid container justifyContent="center" alignContent="center" alignItems="center" spacing={2}>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Location
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { location }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Date/Time
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { convertDateTimeOffsetToStringFormat(createdDate) }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                                Completed By
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            <TopSectionText>
                                { createdBy }
                            </TopSectionText>
                        </Grid>
                    </Grid>
                    <Grid item md={3} xs={12} sm={12}>
                        <Grid item xs={12}>
                            <TopSectionLabel>
                            SRN
                            </TopSectionLabel>
                        </Grid>
                        <Grid item xs={12}>
                            {showSrnAsLink &&
                            <Link to={'/screening-report/'+srn}>
                                <TopSectionText>
                                    { srn }
                                </TopSectionText>
                            </Link>}
                            {!showSrnAsLink && <TopSectionText>{ srn }</TopSectionText>}
                        </Grid>
                    </Grid>
                </Grid>
            </MiddleSection>
        </>
    )
}

export default ReportHeader;