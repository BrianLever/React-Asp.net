<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BHIReportSection.ascx.cs" Inherits="BHIReportSection" %>

<div class="group noborder section">
	<div class="indicatorReport">
		<asp:Repeater runat="server" ID="rptReportSection">
			<HeaderTemplate>
				<table>
					<thead>
						<tr>
							<th class="w50">
								<asp:Literal runat="server" ID="lblHeader"></asp:Literal>
							</th>
							<th class="w10">
								<asp:Localize ID="Localize2" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_TOTAL_POSITIVE %>' />
							</th>
							<th class="w10 percent">
								<asp:Localize ID="Localize4" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_PERCENT_POSITIVE %>' />
							</th>
							<th class="w10">
								<asp:Localize ID="Localize3" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_TOTAL_NEGATIVE %>' />
							</th>
							<th class="w10 percent">
								<asp:Localize ID="Localize5" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_PERCENT_NEGATIVE %>' />
							</th>
							<th class="w10">
								<asp:Localize ID="Localize6" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_TOTAL%>' />
							</th>
						</tr>
					</thead>
					<tbody>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td class="w50">
						<asp:Label ID="ltrQuestion" runat="server" />
						<p class="indicat">
							<asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
						</p>
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrPositive" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrPositivePercent" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrNegative" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrNegativePerent" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrTotal" />
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="alt">
					<td class="w50">
						<asp:Label ID="ltrQuestion" runat="server" />
						<p class="indicat">
							<asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
						</p>
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrPositive" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrPositivePercent" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrNegative" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrNegativePerent" />
					</td>
					<td class="w10">
						<asp:Literal runat="server" ID="ltrTotal" />
					</td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate>
				</tbody> </table>
                    
			</FooterTemplate>
		</asp:Repeater>

		<div class="questionOnFocus">
			<asp:Repeater runat="server" ID="rptQuestionOnFocus">
				<HeaderTemplate>
					<table>
						<thead>
							<tr>
								<th class="w50">
									<asp:Label runat="server" CssClass="preamble" ID="lblPreamble"></asp:Label>
									<asp:Label runat="server" ID="lblQuestion"></asp:Label>
								</th>
								<th class="w10">
									<asp:Localize ID="Localize2" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_TOTAL_POSITIVE %>' />
								</th>
								<th class="w10 percent">
									<asp:Localize ID="Localize4" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_PERCENT_POSITIVE %>' />
								</th>
								<th class="w10">
									<asp:Localize ID="Localize3" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_TOTAL_NEGATIVE %>' />
								</th>
								<th class="w10 percent">
									<asp:Localize ID="Localize5" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_PERCENT_NEGATIVE %>' />
								</th>
								<th class="w10">
									<asp:Localize ID="Localize6" runat="server" Text='<%$ Resources: TextMessages, REPORT_INDICATOR_HEADER_TOTAL %>' />
								</th>
							</tr>
						</thead>
						<tbody>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td class="w50">
							<asp:Label ID="ltrQuestion" runat="server" />
							<p class="indicat">
								<asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
							</p>
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrPositive" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrPositivePercent" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrNegative" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrNegativePerent" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrTotal" />
						</td>
					</tr>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<tr class="alt">
						<td class="w50">
							<asp:Label ID="ltrQuestion" runat="server" />
							<p class="indicat">
								<asp:Literal ID="ltrScoreIndicates" runat="server"></asp:Literal>
							</p>
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrPositive" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrPositivePercent" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrNegative" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrNegativePerent" />
						</td>
						<td class="w10">
							<asp:Literal runat="server" ID="ltrTotal" />
						</td>
					</tr>
				</AlternatingItemTemplate>
				<FooterTemplate>
					</tbody> </table>
                    
				</FooterTemplate>
			</asp:Repeater>
		</div>
		<div class="copyright">
			<asp:Literal ID="ltrCopyright" runat="server"></asp:Literal>
		</div>
	</div>
</div>
