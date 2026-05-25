export default function normalizePhoneNumber(lead) {
  return {
    ...lead,
    phoneNumber: (lead.phoneNumber || "").replace(/\D/g, ""),
  };
}
