using System.ComponentModel.DataAnnotations;

namespace Proposal.Models
{
    public class CreateModel
    {
        //��đ薼
        [Required(ErrorMessage = "��đ薼����͂��Ă��������B")]
        [MaxLength(20, ErrorMessage = "20�����ȓ��œ��͂��Ă��������B")]
        public string? TeianDaimei { get; set; }

        //��Ă̎��
        [Required(ErrorMessage = "��Ă̎�ނ�I�����Ă��������B")]
        public string? TeianShurui { get; set; }

        //��Ă̋敪
        [Required(ErrorMessage = "��Ă̋敪��I�����Ă��������B")]
        public string? TeianKbn { get; set; }

        //����
        [Required(ErrorMessage = "������I�����Ă��������B")]
        public string? Shozoku { get; set; }

        //���E��
        public string? BuSho { get; set; }

        //�ہE����
        public string? KaBumon { get; set; }

        //�W�E�S��
        public string? KakariTantou { get; set; }

        //�������͑�\��
        [Required(ErrorMessage = "�������͑�\������͂��Ă��������B")]
        [MaxLength(6, ErrorMessage = "6�����ȓ��œ��͂��Ă��������B")]
        public string? ShimeiOrDaihyoumei { get; set; }

        //�O���[�v��
        [MaxLength(10, ErrorMessage = "10�����ȓ��œ��͂��Ă��������B")]
        public string? GroupMei { get; set; }

        //�O���[�v�̑S���@
        public string? GroupZenin1 { get; set; }
        //�O���[�v�̑S�����E���@
        public string? GroupZenin1BuSho { get; set; }
        //�O���[�v�̑S���ہE����@
        public string? GroupZenin1KaBumon { get; set; }
        //�O���[�v�̑S���W�E�S���@
        public string? GroupZenin1KakariTantou { get; set; }

        //�O���[�v�̑S���A
        public string? GroupZenin2 { get; set; }
        //�O���[�v�̑S�����E���A
        public string? GroupZenin2BuSho { get; set; }
        //�O���[�v�̑S���ہE����A
        public string? GroupZenin2KaBumon { get; set; }
        //�O���[�v�̑S���W�E�S���A
        public string? GroupZenin2KakariTantou { get; set; }

        //�O���[�v�̑S���B
        public string? GroupZenin3 { get; set; }
        //�O���[�v�̑S�����E���B
        public string? GroupZenin3BuSho { get; set; }
        //�O���[�v�̑S���ہE����B
        public string? GroupZenin3KaBumon { get; set; }
        //�O���[�v�̑S���W�E�S���B
        public string? GroupZenin3KakariTantou { get; set; }

        //��ꎟ�R���҂��o���ɒ�o����
        public bool DaiijishinsashaHezuIsChecked { get; set; }

        //��ꎟ�R���ҏ���
        [Required(ErrorMessage = "������I�����Ă��������B")]
        public string? DaiijishinsashaShozoku { get; set; }

        //��ꎟ�R���ҕ��E��
        public string? DaiijishinsashaBuSho { get; set; }

        //��ꎟ�R���҉ہE����
        public string? DaiijishinsashaKaBumon { get; set; }

        //��ꎟ�R���Ҏ���
        public string? DaiijishinsashaShimei { get; set; }

        //��ꎟ�R���Ҋ��E
        public string? DaiijishinsashaKanshokun { get; set; }

        //�喱��
        public string? ShumuKa { get; set; }

        //�֌W��
        public string? KankeiKa { get; set; }

        //����E���_
        [Required(ErrorMessage = "����E���_����͂��Ă��������B")]
        [MaxLength(2000, ErrorMessage = "2,000�����ȓ��œ��͂��Ă��������B")]
        public string? GenjyoMondaiten { get; set; }

        //���P��
        [Required(ErrorMessage = "���P�Ă���͂��Ă��������B")]
        [MaxLength(2000, ErrorMessage = "2,000�����ȓ��œ��͂��Ă��������B")]
        public string? Kaizenan { get; set; }

        //���ʁi���{�j
        public string? KoukaJishi { get; set; }

        //����
        [Required(ErrorMessage = "���ʂ���͂��Ă��������B")]
        [MaxLength(2000, ErrorMessage = "2,000�����ȓ��œ��͂��Ă��������B")]
        public string? Kouka { get; set; }

        //�Y�t����
        public IFormFile? TenpuFile1 { get; set; }
        public IFormFile? TenpuFile2 { get; set; }
        public IFormFile? TenpuFile3 { get; set; }
        public IFormFile? TenpuFile4 { get; set; }
        public IFormFile? TenpuFile5 { get; set; }
    }
}